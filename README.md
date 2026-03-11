# FS+IoT — Wind Turbine Control Centre

## Overview

This project is a full-stack IoT monitoring and control system.

It allows operators to:
- Monitor all turbine metrics in real-time with live graphs and dashboards
- Send commands to individual turbines (start, stop, set blade pitch, set reporting interval)
- Receive live alerts when sensor values exceed thresholds
- Review full history of commands and measurements stored in the database

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Frontend | React + TypeScript + Vite + TailwindCSS + DaisyUI |
| Backend | .NET 10 (C#), Entity Framework Core |
| Database | PostgreSQL (Neon) |
| Realtime | StateleSSE.AspNetCore (Server-Sent Events) |
| Messaging | MQTT via Mqtt.Controllers |
| SSE Backplane | Redis (Render) |
| Deployment | Fly.io using Docker |
| CI/CD | GitHub Actions |

---

## Security & Authorization

The system uses **JWT authentication** for secure API access. All command endpoints require a valid token.

### Authentication Flow
- Register or login to receive a JWT access token and a refresh token
- Access token expires after 30 minutes
- Refresh token valid for 7 days — use `/RefreshTokens` to rotate

### Test User

| Username | Password |
|----------|----------|
| TestOperator | TestOperator123! |

Live deployment: [https://fsiotclient.fly.dev](https://fsiotclient.fly.dev)

---

## Environment & Configuration

### Backend

**Framework:** .NET 10
**Database:** PostgreSQL
**ORM:** Entity Framework Core

#### Example `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AppOptions": {
    "DbConnectionString": "your postgres connection string",
    "RenderConnectionString": "your redis connection string",
    "MqttBroker": "your mqtt broker host",
    "Token": "your JWT signing key",
    "Issuer": "your token issuer",
    "Audience": "your token audience"
  },
  "AllowedHosts": "*"
}
```

#### Run Locally

```bash
cd server
dotnet restore
dotnet build
dotnet run --project Api
```

#### Database Migrations

```bash
cd server/DataAccess
dotnet ef migrations add <MigrationName> --startup-project ../Api
dotnet ef database update --startup-project ../Api
```

---

### Frontend

**Framework:** React + TypeScript + Vite
**Styling:** TailwindCSS, DaisyUI
**State:** Jotai
**Charts:** Recharts

#### Run Locally

```bash
cd client
npm install
npm run dev
```

Then open the URL shown in the terminal (usually `http://localhost:5173`).

---

## MQTT Topics

The system communicates with turbines over MQTT using the public broker.

| Direction | Topic | Description |
|-----------|-------|-------------|
| Subscribe (receive) | `farm/TM_FS_IoT/windmill/{turbineId}/telemetry` | Live sensor measurements |
| Subscribe (receive) | `farm/TM_FS_IoT/windmill/{turbineId}/alert` | Threshold alerts |
| Publish (send) | `farm/TM_FS_IoT/windmill/{turbineId}/command` | Control commands |

### Available Commands

| Action | Payload |
|--------|---------|
| Start turbine | `{ "action": "start" }` |
| Stop turbine | `{ "action": "stop", "reason": "maintenance" }` |
| Set blade pitch (0–30°) | `{ "action": "setPitch", "angle": 15.5 }` |
| Set reporting interval (1–60s) | `{ "action": "setInterval", "value": 10 }` |

All commands are **validated server-side**, require authentication, and are **saved to the database** with a timestamp and the operator's user ID.

---

## Realtime Architecture

```
MQTT Broker
    │
    ▼
.NET API (Mqtt.Controllers)
    │  saves to DB
    ▼
PostgreSQL ──► Entity Framework Realtime Interceptor
                    │
                    ▼
              StateleSSE (Redis backplane)
                    │
                    ▼
              React Frontend (SSE)
```

When a measurement or alert is saved to the database, the EF interceptor automatically notifies all connected SSE clients with the updated data. On initial connection, the client receives the full current dataset from the database immediately.

---

## Testing

Tests are written with **xUnit** and use **Testcontainers** to spin up a real PostgreSQL instance.

```bash
cd server
dotnet test
```

### Test Coverage

- `AuthService` — full coverage
  - Register: creates user, hashes password, rejects duplicate emails
  - Login: validates credentials, returns JWT + refresh token, saves refresh token to DB
  - Refresh: rotates refresh token, rejects expired/invalid tokens

### Infrastructure

Tests use a shared `DatabaseFixture` (one container for the whole suite) with per-test transaction rollback for isolation. No mocking — real database behaviour.

---

## CI/CD

### Continuous Integration (GitHub Actions)

Triggered on push and pull requests to `main`.

Jobs:
1. **Server tests** — builds .NET solution and runs xUnit tests with Testcontainers

### Continuous Deployment (Fly.io)

Triggered after CI passes.

- Server deployed via `server/fly.toml` and `server/Dockerfile`
- Client deployed via `client/fly.toml` and `client/Dockerfile`
- Uses `--remote-only` so builds happen on Fly's infrastructure

---

## Features

### Working
- JWT authentication with refresh token rotation
- Live turbine metrics via SSE (Server-Sent Events)
- Real-time alerts with severity levels (critical, warning, info)
- Full measurement history loaded from DB on connect
- Turbine control panel (start, stop, pitch, interval)
- Server-side command validation
- Complete action history with operator and timestamp
- Redis SSE backplane for multi-instance support
- Dashboard overview with farm-wide stats
- Per-turbine detail page with charts (power, wind, rotor speed, vibration, temperature)
- Adjustable chart history window (50 / 100 / 250 / 500 / all)

### Known Limitations
- Alert thresholds are defined by the MQTT broker — there is no API to configure them
- SSE connection requires the user to be on the dashboard first for the turbine list to populate in the sidebar (populated from live data via Jotai atom)
