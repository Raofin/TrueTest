'use client'

import '@/app/globals.css'
import SideBar from '@/app/(admin)/sidebar/page'

interface RootLayoutProps {
  children: React.ReactNode
}

export default function RootLayout({ children }: RootLayoutProps) {
  return (
    <div className="flex h-screen">
      <SideBar />
      <main  suppressHydrationWarning className="flex-grow w-full">{children}</main>
    </div>
  )
}
