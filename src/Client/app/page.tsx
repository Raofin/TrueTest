"use client"
import '../styles/globals.css'
import {useDashboard} from "./DashboardContext";

export default function Component(){
   const {dashboardType}=useDashboard();
   console.log("app page",dashboardType);
    return(
        <>
            <div className="flex justify-center items-center w-full mt-24 text-xl">Welcome to OPS</div>
        </>

    )
}