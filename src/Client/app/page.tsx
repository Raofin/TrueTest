'use client'

import '@/app/globals.css'
import { Link } from '@heroui/react'
import Login from '@/app/(auth)/signin/page'
import Logo from '@/components/ui/logo/page'
import NavBar from '@/app/(auth)/NavBar'
import { useEffect } from 'react'
import { getAuthToken } from '@/lib/auth'
import { useAuth } from '@/context/AuthProvider'
import { useRouter } from 'next/navigation'
import ROUTES from '@/constants/route'

export default function Component() {
  const router=useRouter()
   const { user:authenticatedUser } = useAuth()
  useEffect(()=>{
    if (getAuthToken()) {
      console.log(authenticatedUser?.roles)
      if (authenticatedUser?.roles.includes('Admin')) {
        router.push(ROUTES.OVERVIEW)
      } else {
        router.push(ROUTES.HOME)
      }
    } else{
      router.push(ROUTES.SIGN_IN)
    }
  },[authenticatedUser?.roles, router])
  return (
    <div className="h-full flex flex-col justify-between">
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
      <footer className="w-full px-5 my-2">
      <div className="flex justify-between items-center text-gray-400">
          <p>© 2025 TrueTest. All rights reserved.</p>
          <p>
            Contact Us: <Link href="#">support@truetest.com</Link>
          </p>
        </div>
      </footer>
    </div>
  )
}
