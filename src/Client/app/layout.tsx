'use client'

import { Providers } from './providers'

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body className='bg-[#eeeef0] dark:bg-[#000000]'>
        <Providers>{children}</Providers>
      </body>
    </html>
  )
}
