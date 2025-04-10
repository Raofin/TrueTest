'use client'

import React from 'react'
import '@/app/globals.css'
import NavBar from '@/app/navigation-header/NavBar'

import Logo from '@/components/ui/logo/page'
import { Link, Navbar, NavbarContent, NavbarItem } from '@heroui/react'

export default function RootNavBar() {
  return (
    <div className="flex w-full justify-between items-center bg-[#eeeef0] dark:bg-[#000000]">
      <div className="bg-[#eeeef0] dark:bg-[#000000] mt-2">
        <Logo />
      </div>
      <div className="flex">
        <Navbar
          classNames={{
            wrapper: ' justify-end bg-[#eeeef0] dark:bg-[#000000]',
            item: 'hidden md:flex',
          }}
        >
          <NavbarContent className="h-11 gap-5 rounded-full bg-[#ffffff] px-4 dark:bg-[#18181b]" justify="end">
            <NavbarItem>
              <Link className="text-default-500" href="/home" size="sm">
                Home
              </Link>
            </NavbarItem>
            <NavbarItem>
              <Link className="text-default-500" href="/my-exams" size="sm">
                My Exams
              </Link>
            </NavbarItem>
          </NavbarContent>
        </Navbar>
        <NavBar />
      </div>
    </div>
  )
}
