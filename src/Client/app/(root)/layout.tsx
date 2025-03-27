'use client'

import '@/app/globals.css'
import withProtectedRoute from '@/app/components/utils/withProtectedRoute '


const RootLayout=({ children }: {children: React.ReactNode})=> {
  return (
    <div className="flex min-h-screen ">
      <main  suppressHydrationWarning className="flex-grow w-full ">{children}</main>
    </div>
  )
}
export default withProtectedRoute(RootLayout);
