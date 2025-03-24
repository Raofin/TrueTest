'use client'

import {Link, Navbar, NavbarContent, NavbarItem } from '@heroui/react'
import { FaCircleUser } from 'react-icons/fa6'
import { BiSolidLogIn } from 'react-icons/bi'
import Logo from '@/app/components/ui/logo/page'
import ThemeSwitch from '../ThemeSwitch'


export default function Component() {
  return (
    <div className=" w-full flex justify-between items-center h-16 px-2">
     <div className='bg-[#eeeef0] dark:bg-[#000000]'><Logo /></div> 
     <div className='flex'>
           <Navbar
              classNames={{
                wrapper: 'justify-end bg-[#eeeef0] dark:bg-[#000000]',
                item: 'hidden md:flex',
              }}
            >
              <NavbarContent
                className="h-11 gap-5 rounded-full bg-[#ffffff] px-4 dark:bg-[#18181b]"
                justify="end"
              >
                <NavbarItem>
                  <Link className="text-black dark:text-white light:text-black" href="/overview">
                    Overview
                  </Link>
                </NavbarItem>
                <NavbarItem>
                  <Link className="text-black dark:text-white light:text-black" href="/home">
                    Home
                  </Link>
                </NavbarItem>
                <NavbarItem>
                  <Link className="text-black dark:text-white light:text-black" href="/">
                    About Us
                  </Link>
                </NavbarItem>
                <NavbarItem>
                  <Link className="text-black dark:text-white light:text-black" href="/">
                    Contact
                  </Link>
                </NavbarItem>
                <NavbarItem>
                  <Link className="text-black dark:text-white light:text-black" href="/">
                    Support
                  </Link>
                </NavbarItem>
                <NavbarItem>
                  <Link className="text-black dark:text-white light:text-black" href="/">
                    FAQs
                  </Link>
                </NavbarItem>
              </NavbarContent>
           
          
            <NavbarContent
                 className="h-11 gap-5 rounded-full bg-[#ffffff] px-4 dark:bg-[#18181b]" justify="end"
              >
                <NavbarItem className="text-black dark:text-white "><ThemeSwitch/></NavbarItem>
              
               <NavbarItem>
               <Link href="/userprofile/setup"> <FaCircleUser className="text-black dark:text-white " size={24} /></Link>
              </NavbarItem>
               <NavbarItem>
               <Link href="/login">  <BiSolidLogIn className="text-black dark:text-white " size={24} /></Link>
              </NavbarItem>
            </NavbarContent>
            </Navbar>
            </div>
    </div>
  )
}
