'use client'

import React from 'react'
import 'app/globals.css'
import NavBar from 'app/navigation-header/NavBar'
import { ThemeProvider } from 'app/ThemeProvider'

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <div>
      <NavBar />
      <ThemeProvider>{children}</ThemeProvider>
    </div>
  )
}
