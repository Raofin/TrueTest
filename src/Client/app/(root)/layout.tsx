'use client'

import React from 'react'
import '@/app/globals.css'
import NavBar from '@/app/navigation-header/NavBar'

import Logo from '@/app/components/ui/logo/page'
import { Link, Navbar, NavbarContent, NavbarItem, NavbarProps } from '@heroui/react'

interface RootLayoutProps extends NavbarProps {
  children: React.ReactNode
}

export default function RootLayout({ children, ...props }: RootLayoutProps) {
  return (
    <div>
      <div className="flex w-full justify-between">
        <Logo />
        <div className="flex">
          <Navbar
            {...props}
            classNames={{
              wrapper: ' justify-end bg-transparent',
              item: 'hidden md:flex',
            }}
          >
            <NavbarContent
              className="hidden h-11 gap-4 rounded-full border-small border-default-200/20 bg-background/60 px-4 shadow-medium backdrop-blur-md backdrop-saturate-150 dark:bg-default-100/50 md:flex"
              justify="end"
            >
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
      {children}
    </div>
  )
}
