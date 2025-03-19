'use client'

import { ThemeProvider } from 'next-themes'
import { AuthProvider } from './context/AuthProvider'
import { DashboardProvider } from './context/DashboardContext'

export function Providers({ children }: { children: React.ReactNode }) {
  return (
    <ThemeProvider attribute="class" defaultTheme="system" enableSystem>
      <AuthProvider>
        <DashboardProvider>{children}</DashboardProvider>
      </AuthProvider>
    </ThemeProvider>
  )
}
