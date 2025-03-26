'use client'

import { Providers } from '@/app/providers'
import { Inter } from 'next/font/google'
const inter = Inter({ subsets: ['latin'] })

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html className={inter.className} lang="en" suppressHydrationWarning>
      <body className='bg-[#eeeef0] dark:bg-[#000000]'>
        <Providers>{children}</Providers>
      </body>
    </html>
  )
}
