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
            wrapper: 'justify-end bg-[#eeeef0] dark:bg-[#000000]',
            item: 'hidden md:flex',
          }}
        >
          <NavbarContent
            className=" h-11 gap-5 rounded-full bg-[#ffffff] px-4 dark:bg-[#18181b]  px-4 "
            justify="end"
          >
            <NavbarItem>
              <Link className="text-black dark:text-white" href="/overview">
                Admin Panel
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-black dark:text-white" href="/home">
                Candidate Home
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-black dark:text-white" href="/">
                About Us
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-black dark:text-white" href="/">
                Contact
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-black dark:text-white" href="/">
                Support
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-black dark:text-white" href="/">
                FAQs
              </Link>
            </NavbarItem>
          </NavbarContent>
       
      
        <NavbarContent
            className="hidden h-11 gap-5 rounded-full bg-[#ffffff] px-4 backdrop-blur-md backdrop-saturate-150 dark:bg-default-100/50 md:flex"
            justify="end"
          >
            <NavbarItem className=" light:text-white"><ThemeSwitch /></NavbarItem>
          
           <NavbarItem>
           <Link href="/userprofile/setup"> <FaCircleUser className="text-black dark:text-white " size={24} /></Link>
          </NavbarItem>
           <NavbarItem>
            <BiSolidLogIn className=" light:text-white" size={24} />
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
        <div className="flex justify-between items-center text-black py-4">
          <p>© 2025 TrueTest. All rights reserved.</p>
          <p>
            Contact Us: <Link href="#">support@truetest.com</Link>
          </p>
        </div>
      </footer>
    </>
  )
}
