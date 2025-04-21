'use client'

import { Link, Navbar, NavbarContent, NavbarItem } from '@heroui/react'
import Logo from '@/components/ui/logo/page'
import ThemeSwitch from '../ThemeSwitch'

export default function Component() {
  return (
    <div className=" w-full flex justify-between items-center h-16 px-2">
      <div className="bg-[#eeeef0] dark:bg-[#000000]">
        <Logo />
      </div>
      <div className="flex">
        <Navbar
          classNames={{
            wrapper: 'justify-end bg-[#eeeef0] dark:bg-[#000000]',
            item: 'hidden md:flex',
          }}
        >
          <NavbarContent className=" h-11 gap-5 rounded-full bg-[#ffffff] px-4 dark:bg-[#18181b]  px-4 " justify="end">
            <NavbarItem>
              <Link className="text-[#3f3f46] dark:text-white" href="/">
                About Us
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-[#3f3f46] dark:text-white" href="/">
                Contact
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-[#3f3f46] dark:text-white" href="/">
                Support
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-[#3f3f46] dark:text-white" href="/">
                FAQs
              </Link>
            </NavbarItem>
          </NavbarContent>
          <NavbarContent className="h-11 gap-5 rounded-full bg-[#ffffff] px-4 dark:bg-[#18181b]" justify="end">
            <NavbarItem className="text-[#3f3f46] dark:text-white ">
              <ThemeSwitch />
            </NavbarItem>
           
          </NavbarContent>
        </Navbar>
      </div>
    </div>
  )
}
