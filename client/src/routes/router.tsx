import {createBrowserRouter} from "react-router";

import Layout from "../layout/Layout.tsx";
import PlayerDashboard from "../pages/player/PlayerDashboard.tsx";
import LoginPage from "../pages/LoginPage.tsx";

import ProtectedRoute from "./ProtectedRoute.tsx";
import RegisterPage from "../pages/RegisterPage.tsx";

export const router = createBrowserRouter([
    {
        path: "/",
        element: (
            <ProtectedRoute>
                <Layout />
            </ProtectedRoute>
        ),
        children: [

            {
                index: true,
                element: <PlayerDashboard />
            },
        ]
    },

    {
        path: "/login",
        element: <LoginPage />
    },
    {
        path: "/register",
        element: <RegisterPage />
    },
]);