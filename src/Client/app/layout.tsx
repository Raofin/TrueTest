'use client'

import React from 'react'
import { Providers } from './providers'
import { ThemeProvider } from './ThemeProvider'

const RootLayout = React.memo(function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <Providers>
          <ThemeProvider>
            <main>{children}</main>
          </ThemeProvider>
        </Providers>
      </body>
    </html>
  )
})

export default RootLayout
