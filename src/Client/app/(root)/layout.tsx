'use client'

import '@/app/globals.css'
// import withProtectedRoute from '@/app/components/utils/withProtectedRoute '

interface RootLayoutProps {
  readonly children: React.ReactNode
}

const RootLayout = ({ children }: RootLayoutProps) => {
  return (
    <div className="flex min-h-screen ">
      <main suppressHydrationWarning className="flex-grow w-full ">
        {children}
      </main>
    </div>
  )
}
// export default withProtectedRoute(RootLayout)
export default RootLayout
