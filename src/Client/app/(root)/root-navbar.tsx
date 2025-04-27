'use client'

import React from 'react'
import '@/app/globals.css'
import NavBar from '@/app/navigation-header/NavBar'

import Logo from '@/components/ui/TrueTestLogo'
import { Navbar, NavbarContent, NavbarItem } from '@heroui/react'
import { useRouter } from 'next/navigation'

export default function RootNavBar() {
  const router=useRouter()
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
            <NavbarItem style={{cursor:'pointer'}} onClick={()=>router.push('/home')}>
                Home
            </NavbarItem>
            <NavbarItem  style={{cursor:'pointer'}} onClick={()=>router.push('/my-exams')}>
                My Exams
            </NavbarItem>
          </NavbarContent>
        </Navbar>
        <NavBar />
      </div>
    </div>
  )
}
