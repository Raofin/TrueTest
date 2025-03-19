'use client'

import '@/app/globals.css'
import { Link, Navbar, NavbarContent, NavbarItem } from '@heroui/react'
import Login from '@/app/(auth)/login/page'
import Logo from '@/app/components/ui/logo/page'
import { FaCircleUser } from 'react-icons/fa6'
import { BiSolidLogIn } from 'react-icons/bi'
import ThemeSwitch from './ThemeSwitch'


export default function Component() {
  return (
    <>
      <div className=" w-full flex justify-between items-center h-16">
        <div className="flex items-center gap-2">
          <Logo />
        </div>
       <div className='flex'>
       <Navbar
          classNames={{
            wrapper: 'justify-end bg-transparent',
            item: 'hidden md:flex',
          }}
        >
          <NavbarContent
            className="hidden h-11 gap-5 rounded-full border-small border-default-200/20 bg-background/60 px-4 shadow-medium backdrop-blur-md backdrop-saturate-150 dark:bg-default-100/50 md:flex"
            justify="end"
          >
            <NavbarItem>
              <Link className="text-gray-400 light:text-black" href="/overview">
                Overview
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-gray-400 light:text-black" href="/home">
                Home
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-gray-400 light:text-black" href="/">
                About Us
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-gray-400 light:text-black" href="/">
                Contact
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-gray-400 light:text-black" href="/">
                Support
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-gray-400 light:text-black" href="/">
                FAQs
              </Link>
            </NavbarItem>
          </NavbarContent>
       
      
        <NavbarContent
            className="hidden h-11 gap-5 rounded-full border-small border-default-200/20 bg-background/60 px-4 shadow-medium backdrop-blur-md backdrop-saturate-150 dark:bg-default-100/50 md:flex"
            justify="end"
          >
            <NavbarItem className="text-gray-400 light:text-white"><ThemeSwitch /></NavbarItem>
          
           <NavbarItem>
            <FaCircleUser className="text-gray-400 light:text-white" size={24} />
          </NavbarItem>
           <NavbarItem>
            <BiSolidLogIn className="text-gray-400 light:text-white" size={24} />
          </NavbarItem>
        </NavbarContent>
        </Navbar>
        </div>
      </div>
      <div className="flex justify-around items-center gap-12 ">
        <div className="flex flex-col items-center gap-4 flex-1">
          <div className="flex items-center gap-2">
            <Logo />
            <p className="font-extrabold text-3xl">
              <span className="text-red-500">True</span>
              <span className="text-blue-500">Test</span>
            </p>
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
        <div className="flex justify-between items-center text-gray-400 py-4">
          <p>© 2025 TrueTest. All rights reserved.</p>
          <p>
            Contact Us: <Link href="#">support@truetest.com</Link>
          </p>
        </div>
      </footer>
    </>
  )
}
