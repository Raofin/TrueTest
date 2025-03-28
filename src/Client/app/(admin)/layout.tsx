'use client'

import '@/app/globals.css'
import SideBar from '@/app/(admin)/sidebar/page'
import withProtectedRoute from '@/app/components/utils/withProtectedRoute '

interface RootLayoutProps {
  readonly children: React.ReactNode
}

const AdminLayout = ({ children }: RootLayoutProps) => {
  return (
    <div className="flex min-h-screen ">
      <SideBar />
      <main suppressHydrationWarning className="flex-grow w-full ">
        {children}
      </main>
    </div>
  )
}
export default withProtectedRoute(AdminLayout)
