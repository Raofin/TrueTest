'use client'
import { createContext, useContext, useState, useEffect, ReactNode } from "react";
import {usePathname} from "next/navigation";

interface DashboardContextProps {
    dashboardType: "candidate" | "admin" |null;
}

const DashboardContext = createContext<DashboardContextProps | undefined>(undefined);

interface DashboardProviderProps {
    children: ReactNode;
}

export const DashboardProvider: React.FC<DashboardProviderProps> = ({ children }) => {

    const [dashboardType, setDashboardType] = useState<"candidate" | "admin" |null>(null);
  const path= usePathname();

    useEffect(() => {
        if (path.includes("candidate")) {
            setDashboardType("candidate");
        } else if (path.includes("admin")) {
            setDashboardType("admin");
        } 
    }, [path]);
    return (
        <DashboardContext.Provider value={{ dashboardType }}>
            {children}
        </DashboardContext.Provider>
    );
};

export const useDashboard = (): DashboardContextProps => {
    const context = useContext(DashboardContext);
    if (!context) {
        throw new Error("Error");
    }
    return context;
};
