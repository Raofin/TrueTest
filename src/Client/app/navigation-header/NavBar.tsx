'use client'

import React from 'react'
import {
  Link,
  Dropdown,
  DropdownTrigger,
  DropdownMenu,
  DropdownItem,
  Popover,
  PopoverContent,
  PopoverTrigger,
  Avatar,
  NavbarContent,
  NavbarItem,
  Badge,
  Navbar,
  MergeWithAs,
  NavbarProps,
} from '@heroui/react'
import { Icon } from '@iconify/react'
import NotificationsCard from 'app/navigation-header/notifications-card'
import Logo from 'app/components/ui/logo/page'
import ThemeToggle from 'app/components/ThemeToggle'
import { usePathname } from 'next/navigation'

export default function Component(
  props: React.JSX.IntrinsicAttributes &
    MergeWithAs<
      Omit<React.DetailedHTMLProps<React.HTMLAttributes<HTMLDivElement>, HTMLDivElement>, 'ref'>,
      Omit<Omit<React.DetailedHTMLProps<React.HTMLAttributes<HTMLDivElement>, HTMLDivElement>, 'ref'>, never>,
      NavbarProps,
      'div'
    >
) {
  const pathname = usePathname()
  return (
    <div className="mx-1">
      <div className="w-full flex justify-between items-center">
        <div>
          <Logo />
        </div>
        <div className="flex gap-5 justify-end items-center">
          {(pathname.includes('home') && pathname.includes('my-exams') )? (
            <Navbar
              {...props}
              classNames={{
                wrapper: 'w-full justify-end bg-transparent',
                item: 'hidden md:flex',
              }}
            >
              <NavbarContent
                className="hidden h-11 gap-4 rounded-full border-small border-default-200/20 bg-background/60 px-4 shadow-medium backdrop-blur-md backdrop-saturate-150 dark:bg-default-100/50 md:flex"
                justify="end"
              >
                <NavbarItem>
                  <Link className="text-default-500" href="/root/home" size="sm">
                    Home
                  </Link>
                </NavbarItem>
                <NavbarItem>
                  <Link className="text-default-500" href="/root/myexams" size="sm">
                    My Exams
                  </Link>
                </NavbarItem>
              </NavbarContent>
              <NavbarContent
                className="hidden h-11 gap-2 rounded-full border-small border-default-200/20 bg-background/60 px-4 shadow-medium backdrop-blur-md backdrop-saturate-150 dark:bg-default-100/50 md:flex"
                justify="end"
              >
                <NavbarItem>
                  <ThemeToggle />
                </NavbarItem>
                <NavbarItem>
                  <Popover offset={12} placement="bottom-end">
                    <PopoverTrigger className="rounded-full p-2 hover:bg-gray-200 transition-colors duration-300 cursor-pointer">
                      <Icon className="text-gray-600" icon="solar:bell-linear" width={40} />
                    </PopoverTrigger>
                    <PopoverContent className="max-w-[90vw] p-0 sm:max-w-[380px]">
                      <NotificationsCard className="w-full shadow-none" />
                    </PopoverContent>
                  </Popover>
                </NavbarItem>
                <NavbarItem>
                  <button className="">
                    <Dropdown placement="bottom-end">
                      <DropdownTrigger>
                        <div className="mt-1 h-8 w-8 outline-none transition-transform">
                          <Badge
                            className="border-transparent"
                            color="success"
                            content=""
                            placement="bottom-right"
                            shape="circle"
                            size="sm"
                          >
                            <Avatar size="sm" src="" alt="User Avatar" />
                          </Badge>
                        </div>
                      </DropdownTrigger>
                      <DropdownMenu aria-label="Profile Actions" variant="flat">
                        <DropdownItem key="user" className="h-14 gap-2">
                          <div className="flex gap-2 mb-2">
                            <Avatar size="md" src="" alt="User Avatar" />
                            <div>
                              <p>username</p>
                              <p>useremail@gmail.com</p>
                            </div>
                          </div>
                          <hr />
                        </DropdownItem>
                        <DropdownItem key="profile">
                          <Link
                            href="/profile"
                            className="flex items-center gap-2"
                            style={{ color: 'inherit', textDecoration: 'none' }}
                          >
                            <Icon icon="lucide:user" className="w-5 h-5" />
                            My Profile
                          </Link>
                        </DropdownItem>

                        <DropdownItem key="settings">
                          <Link
                            href="/settings/1"
                            className={`flex items-center gap-2`}
                            style={{ color: 'inherit', textDecoration: 'none' }}
                          >
                            <Icon icon="lucide:settings" className="w-5 h-5" />
                            Account Settings
                          </Link>
                        </DropdownItem>

                        <DropdownItem key="logout">
                          <Link
                            href="/login"
                            className={`flex items-center gap-2 `}
                            style={{ color: 'inherit', textDecoration: 'none' }}
                          >
                            <Icon icon="lucide:log-out" className="w-5 h-5" />
                            <p>Logout</p>
                          </Link>
                        </DropdownItem>
                      </DropdownMenu>
                    </Dropdown>
                  </button>
                </NavbarItem>
              </NavbarContent>
            </Navbar>
          ) : (
            <Navbar
              {...props}
              classNames={{
                wrapper: 'w-full justify-end bg-transparent',
                item: 'hidden md:flex',
              }}
            >
              <NavbarContent
                className="hidden h-11 gap-2 rounded-full border-small border-default-200/20 bg-background/60 px-4 shadow-medium backdrop-blur-md backdrop-saturate-150 dark:bg-default-100/50 md:flex"
                justify="end"
              >
                <NavbarItem>
                  <ThemeToggle />
                </NavbarItem>
                <NavbarItem>
                  <Popover offset={12} placement="bottom-end">
                    <PopoverTrigger className="rounded-full p-2 hover:bg-gray-200 transition-colors duration-300 cursor-pointer">
                      <Icon className="text-gray-600" icon="solar:bell-linear" width={40} />
                    </PopoverTrigger>
                    <PopoverContent className="max-w-[90vw] p-0 sm:max-w-[380px]">
                      <NotificationsCard className="w-full shadow-none" />
                    </PopoverContent>
                  </Popover>
                </NavbarItem>
                <NavbarItem>
                  <button className="">
                    <Dropdown placement="bottom-end">
                      <DropdownTrigger>
                        <div className="mt-1 h-8 w-8 outline-none transition-transform">
                          <Badge
                            className="border-transparent"
                            color="success"
                            content=""
                            placement="bottom-right"
                            shape="circle"
                            size="sm"
                          >
                            <Avatar size="sm" src="" alt="User Avatar" />
                          </Badge>
                        </div>
                      </DropdownTrigger>
                      <DropdownMenu aria-label="Profile Actions" variant="flat">
                        <DropdownItem key="user" className="h-14 gap-2">
                          <div className="flex gap-2 mb-2">
                            <Avatar size="md" src="" alt="User Avatar" />
                            <div>
                              <p>username</p>
                              <p>useremail@gmail.com</p>
                            </div>
                          </div>
                          <hr />
                        </DropdownItem>
                        <DropdownItem key="profile">
                          <Link
                            href="/profile"
                            className="flex items-center gap-2"
                            style={{ color: 'inherit', textDecoration: 'none' }}
                          >
                            <Icon icon="lucide:user" className="w-5 h-5" />
                            My Profile
                          </Link>
                        </DropdownItem>

                        <DropdownItem key="settings">
                          <Link
                            href="/settings/1"
                            className={`flex items-center gap-2`}
                            style={{ color: 'inherit', textDecoration: 'none' }}
                          >
                            <Icon icon="lucide:settings" className="w-5 h-5" />
                            Account Settings
                          </Link>
                        </DropdownItem>

                        <DropdownItem key="logout">
                          <Link
                            href="/login"
                            className={`flex items-center gap-2 `}
                            style={{ color: 'inherit', textDecoration: 'none' }}
                          >
                            <Icon icon="lucide:log-out" className="w-5 h-5" />
                            <p>Logout</p>
                          </Link>
                        </DropdownItem>
                      </DropdownMenu>
                    </Dropdown>
                  </button>
                </NavbarItem>
              </NavbarContent>
            </Navbar>
          )}
        </div>
      </div>
    </div>
  )
}
