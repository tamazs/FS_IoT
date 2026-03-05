import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { useMeasurementsSSE, useAlertsSSE } from "../hooks/useSse";
import type { Measurement } from "../generated-ts-client";
import StatCard from "../components/ui/StatCard";
import CommandPanel from "../components/turbine/CommandPanel";
import AlertList from "../components/dashboard/AlertList";
import { MetricChart, TemperatureChart } from "../components/charts/MetricChart";

export default function TurbinePage() {
    const { turbineId } = useParams<{ turbineId: string }>();
    const [history, setHistory] = useState<Measurement[]>([]);
    const [tab, setTab] = useState<"metrics" | "alerts">("metrics");

    const allMeasurements = useMeasurementsSSE();
    const allAlerts = useAlertsSSE();

    const liveMeasurements = allMeasurements?.filter((m) => m.turbineId === turbineId) ?? [];
    const alerts = allAlerts?.filter((a) => a.turbineId === turbineId) ?? [];
    const latest = liveMeasurements[liveMeasurements.length - 1] ?? null;

    // Build chart history from incoming SSE data
    useEffect(() => {
        if (liveMeasurements.length > 0) {
            setHistory((prev) => {
                const combined = [...prev, ...liveMeasurements];
                const seen = new Set<string>();
                return combined.filter((m) => !seen.has(m.id) && seen.add(m.id) as unknown as boolean).slice(-60);
            });
        }
    }, [allMeasurements]);

    function statusColor(s?: string) {
        const map: Record<string, string> = { running: "badge-success", stopped: "badge-error", maintenance: "badge-warning" };
        return map[s?.toLowerCase() ?? ""] ?? "badge-neutral";
    }

    return (
        <div className="p-6 space-y-6">
            <div>
                <div className="flex items-center gap-2">
                    <h1 className="text-2xl font-bold">{latest?.turbineName ?? turbineId}</h1>
                    {latest && <span className={`badge ${statusColor(latest.status)}`}>{latest.status}</span>}
                    {latest && <span className="badge badge-outline badge-xs animate-pulse">LIVE</span>}
                </div>
                <p className="text-base-content/50 text-sm mt-0.5">{turbineId} · Farm {latest?.farmId ?? "—"}</p>
            </div>

            <div className="grid grid-cols-1 xl:grid-cols-4 gap-6">
                <div className="xl:col-span-3 space-y-5">
                    <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
                        <StatCard label="Power Output" value={latest?.powerOutput.toFixed(1) ?? "—"} unit="kW" color="success" />
                        <StatCard label="Wind Speed" value={latest?.windSpeed.toFixed(1) ?? "—"} unit="m/s" color="info" />
                        <StatCard label="Rotor Speed" value={latest?.rotorSpeed.toFixed(1) ?? "—"} unit="RPM" color="primary" />
                        <StatCard label="Blade Pitch" value={latest?.bladePitch.toFixed(1) ?? "—"} unit="°" color="warning" />
                    </div>
                    <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
                        <StatCard label="Generator Temp" value={latest?.generatorTemp.toFixed(1) ?? "—"} unit="°C" color={latest && latest.generatorTemp > 80 ? "error" : "primary"} />
                        <StatCard label="Gearbox Temp" value={latest?.gearboxTemp.toFixed(1) ?? "—"} unit="°C" color={latest && latest.gearboxTemp > 70 ? "error" : "primary"} />
                        <StatCard label="Vibration" value={latest?.vibration.toFixed(2) ?? "—"} unit="g" color={latest && latest.vibration > 5 ? "warning" : "primary"} />
                        <StatCard label="Nacelle Dir." value={latest?.nacelleDirection.toFixed(0) ?? "—"} unit="°" />
                    </div>

                    <div className="tabs tabs-boxed w-fit">
                        <button className={`tab ${tab === "metrics" ? "tab-active" : ""}`} onClick={() => setTab("metrics")}>Metrics</button>
                        <button className={`tab ${tab === "alerts" ? "tab-active" : ""}`} onClick={() => setTab("alerts")}>
                            Alerts
                            {alerts.filter(a => a.severity === "critical").length > 0 && (
                                <span className="badge badge-error badge-xs ml-1">{alerts.filter(a => a.severity === "critical").length}</span>
                            )}
                        </button>
                    </div>

                    {tab === "metrics" && (
                        <div className="space-y-4">
                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                                <MetricChart data={history} metric="powerOutput" label="Power Output" unit="kW" color="#22c55e" />
                                <MetricChart data={history} metric="windSpeed" label="Wind Speed" unit="m/s" color="#3b82f6" />
                                <MetricChart data={history} metric="rotorSpeed" label="Rotor Speed" unit="RPM" color="#570df8" />
                                <MetricChart data={history} metric="vibration" label="Vibration" unit="g" color="#f59e0b" />
                            </div>
                            <TemperatureChart data={history} />
                        </div>
                    )}

                    {tab === "alerts" && (
                        <div className="card bg-base-100 shadow-sm border border-base-300">
                            <div className="card-body p-4">
                                <AlertList alerts={alerts} />
                            </div>
                        </div>
                    )}
                </div>

                <div className="xl:col-span-1">
                    <CommandPanel turbineId={turbineId!} />
                </div>
            </div>
        </div>
    );
}