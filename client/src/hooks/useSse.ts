import { useEffect, useState } from "react";
import { StateleSSEClient } from "statele-sse";
import {type Measurement, type Alert, WebClientClient} from "../generated-ts-client";
import {finalUrl} from "../baseUrl.ts";
import {customFetch} from "./customFetch.ts";

// Shared SSE client instance
const sseClient = new StateleSSEClient(`${finalUrl}/sse`);

const webClient = new WebClientClient(finalUrl, customFetch);

export function useMeasurementsSSE() {
    const [measurements, setMeasurements] = useState<Measurement[] | null>(null);

    useEffect(() => {
        sseClient.listen(
            async (connectionId) => {
                const result = await webClient.getMeasurements(connectionId);
                return result;
            },
            (data) => {
                setMeasurements(data ?? null);
            }
        );
    }, []);

    return measurements;
}

export function useAlertsSSE() {
    const [alerts, setAlerts] = useState<Alert[] | null>(null);

    useEffect(() => {
        sseClient.listen(
            async (connectionId) => {
                const result = await webClient.getAlerts(connectionId);
                return result;
            },
            (data) => {
                setAlerts(data ?? null);
            }
        );
    }, []);

    return alerts;
}

