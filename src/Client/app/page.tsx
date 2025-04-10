'use client'

import '@/app/globals.css'
import { Link } from '@heroui/react'
import Login from '@/app/(auth)/signin/page'
import Logo from '@/components/ui/logo/page'
import NavBar from '@/app/(auth)/NavBar'

export default function Component() {
  return (
    <div className="h-screen flex flex-col justify-between">
      <div className=" w-full flex justify-between items-center h-16">
        <NavBar />
      </div>
      <div className="flex justify-around items-center gap-12 ">
        <div className="flex flex-col items-center gap-4 flex-1">
          <div className="flex items-center gap-2">
            <Logo />
          </div>
          <div className="flex flex-col items-center justify-center text-5xl font-bold ">
            <p> Your Secure Platform</p>
            <p>for Assessments</p>
          </div>
          <p className="text-[#71717A] light:text-black max-w-md text-center">
            TrueTest ensures a fair, secure environment with real-time proctoring and anti-cheating features, so you can
            focus on showcasing your skills!
          </p>
        </div>
        <div className="flex-1">
          <Login />
        </div>
      </div>
      <footer className="w-full h-12 px-5 py-4 mb-5 pb-5">
        <div className="flex justify-between items-center text-[#3f3f46] dark:text-white py-4">
          <p>© 2025 TrueTest. All rights reserved.</p>
          <p>
            Contact Us: <Link href="#">support@truetest.com</Link>
          </p>
        </div>
      </footer>
    </div>
  )
}
