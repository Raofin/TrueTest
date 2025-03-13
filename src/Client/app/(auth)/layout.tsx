'use client'
import NavBar from './NavBar'
import '../../styles/globals.css'
import React from 'react';
import Logo from '../components/ui/logo/page'
import { Link } from '@heroui/react';
export default function RootLayout({ children }: { children: React.ReactNode }) {

    return (
        <>
            <NavBar />
            <div className='flex justify-around items-center gap-12'>
                    <div className="flex flex-col items-center gap-4 flex-1">
                      <div className="flex items-center gap-2 text-2xl">
                        <Logo />
                       
                      </div>
                      <div className='flex flex-col items-center justify-center text-5xl font-bold '>
                        <p> Your Secure Platform</p>
                        <p>for Assessments</p>
                      </div>
                      <p className="text-[#71717A] max-w-md text-center">
                        TrueTest ensures a fair, secure environment with real-time proctoring and anti-cheating features, so you can focus on showcasing your skills!
                      </p>
                    </div>
                    <div className='flex-1'>
                     {children}
                    </div>
                  </div>
                  <footer className='w-full h-12 px-5 py-4 mb-5 pb-5'>
                        <div className='flex justify-between items-center text-gray-400'>
                            <p>© 2025 TrueTest. All rights reserved.</p>
                            <p>Contact Us: <Link href="#">support@truetest.com</Link></p>
                        </div>
                    </footer>
        </>
    )
}