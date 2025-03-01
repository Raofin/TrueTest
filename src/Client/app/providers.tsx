"use client";

// import { useEffect } from "react";
// import { AuthProvider, useAuth } from "./context/AuthProvider"; 
// import { useRouter, usePathname } from "next/navigation";
import { DashboardProvider } from "./context/DashboardContext";

export function Providers({ children }: { children: React.ReactNode }) {
  return (
    // <AuthProvider>
      // <AuthWrapper>
        <DashboardProvider>{children}</DashboardProvider>
      // </AuthWrapper>
    // </AuthProvider>
  );
}

// function AuthWrapper({ children }: { children: React.ReactNode }) {
//   const { user } = useAuth();
//   const router = useRouter();
//   const pathname = usePathname(); 

//   const publicRoutes = ["/auth/login", "/auth/register"];

//   useEffect(() => {
//     if (user === null && !publicRoutes.includes(pathname)) {
//       router.push("/auth/login");
//     }
//   }, [user, pathname, router]);

//   if (user === null && !publicRoutes.includes(pathname)) return null;

//   return <>{children}</>;
// }