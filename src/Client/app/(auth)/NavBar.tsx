'use client'

import {Link, Navbar, NavbarContent, NavbarItem } from '@heroui/react'
import { FaCircleUser } from 'react-icons/fa6'
import { BiSolidLogIn } from 'react-icons/bi'
import Logo from '@/app/components/ui/logo/page'
import ThemeToggle from '@/app/components/ThemeToggle'

export default function Component() {
  return (
    <div className=" w-full flex justify-between items-center h-16 px-5">
      <Logo />
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
                <NavbarItem><ThemeToggle /></NavbarItem>
              
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
  )
}
