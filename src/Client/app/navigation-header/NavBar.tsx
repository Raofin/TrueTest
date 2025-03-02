"use client";

import { useDashboard } from "../context/DashboardContext";
import { Card, CardBody, Tab, Tabs } from "@heroui/react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSun, faMoon } from '@fortawesome/free-solid-svg-icons';
import {
  Link, Dropdown, DropdownTrigger, DropdownMenu,
  DropdownItem, Popover, PopoverContent, PopoverTrigger, Avatar, Badge, Navbar,
  NavbarBrand, NavbarContent, NavbarItem, NavbarMenuToggle, Button
} from "@nextui-org/react";
import { Icon } from "@iconify/react";
import React, { useState, useEffect } from "react";
import MyExam from '../dashboard/candidate-dashboard/myexams/page';
import Review from '../dashboard/reviewer-dashboard/review-list/page';
import NotificationsCard from "./notifications-card";
import Home from '../dashboard/candidate-dashboard/home/page'
import { usePathname, useRouter } from "next/navigation";

interface PageProps {
  onThemeToggle?: (theme: string) => void;
}


export default function Component({ onThemeToggle }: PageProps) {
  const { dashboardType } = useDashboard();
  const [selected, setSelected] = useState<string>("home");
  const router = useRouter();
  const [isDarkMode, setIsDarkMode] = useState(false);

  useEffect(() => {
    const savedTheme = localStorage.getItem("theme");
    if (savedTheme) {
      setIsDarkMode(savedTheme === "dark");
    }
  }, []);

  const handleThemeChange = () => {
    setIsDarkMode((prev) => !prev);
    const newTheme = !isDarkMode ? "dark" : "light";
    localStorage.setItem("theme", newTheme);
    if (onThemeToggle) {
      onThemeToggle(newTheme);
    }
  };

  const handleLogout = () => {
    router.push('/auth/login');
  };

  const pathname = usePathname();

  return (
    <div className='pt-4'>
      {(dashboardType || pathname.includes("exam")) ? (
        <>
          {dashboardType !== 'admin' && (
            <div className="shadow">
              <h2 className='flex absolute ml-24 text-3xl font-extrabold'>OPS</h2>
              <div className="flex gap-5 absolute justify-center items-center ml-[1060px]">
                <Button
                  isIconOnly
                  radius="full"
                  variant="light"
                  onPress={handleThemeChange}
                  className="hover:bg-gray-200 transition-colors duration-300 cursor-pointer ml-7">
                  <FontAwesomeIcon icon={isDarkMode ? faSun : faMoon} width={30} />
                </Button>

                <Popover offset={12} placement="bottom-end">
                  <PopoverTrigger className="rounded-full p-2 hover:bg-gray-200 transition-colors duration-300 cursor-pointer">
                    <Icon className="text-gray-600" icon="solar:bell-linear" width={35} />
                  </PopoverTrigger>
                  <PopoverContent className="max-w-[90vw] p-0 sm:max-w-[380px]">
                    <NotificationsCard className="w-full shadow-none" />
                  </PopoverContent>
                </Popover>

                <button className="mr-5">
                  <Dropdown placement="bottom-end">
                    <DropdownTrigger>
                      <div className="mt-1 h-8 w-8 outline-none transition-transform">
                        <Badge
                          className="border-transparent"
                          color="success"
                          content=""
                          placement="bottom-right"
                          shape="circle"
                          size="sm">
                          <Avatar
                            size="sm"
                            src="" 
                            alt="User Avatar"
                          />
                        </Badge>
                      </div>
                    </DropdownTrigger>
                    <DropdownMenu aria-label="Profile Actions" variant="flat">
                      <DropdownItem key="user" className="h-14 gap-2">
                        <div className='flex gap-2 mb-2'>
                          <Avatar size="md" src="" alt="User Avatar" />
                          <div>
                            <p>Username</p>
                            <p>useremail@gmail.com</p>
                          </div>
                        </div>
                        <hr />
                      </DropdownItem>

                      <DropdownItem key="profile" className='text-white'>
                        <Link href="/myprofile/1" className="flex items-center gap-2">
                          <Icon icon="lucide:user" className="w-5 h-5" />
                          My Profile
                        </Link>
                      </DropdownItem>

                      <DropdownItem key="settings" className='text-white'>
                        <Link href="/settings/1" className="flex items-center gap-2">
                          <Icon icon="lucide:settings" className="w-5 h-5" />
                          Account Settings
                        </Link>
                      </DropdownItem>

                      <DropdownItem key="logout" className='text-white'>
                        <Link href="/auth/login" className="flex items-center gap-2">
                          <Icon icon="lucide:log-out" className="w-5 h-5" />
                          Logout
                        </Link>
                      </DropdownItem>
                    </DropdownMenu>
                  </Dropdown>
                </button>
              </div>
            </div>
          )}
          <Tabs className={'flex justify-center space-x-7 mx-4'} aria-label="Options" selectedKey={selected} onSelectionChange={(key) => setSelected(key as string)}>
            {dashboardType === 'candidate' ? (
              <>
                <Tab key="home" title="Home">
                <Card>
                    <CardBody>
                      <Home />
                    </CardBody>
                  </Card>
                </Tab>
                <Tab key="My Exams" title="Attend Exam">
                  <Card>
                    <CardBody>
                      <MyExam />
                    </CardBody>
                  </Card>
                </Tab>
              </>
            ) : dashboardType === 'reviewer' ? (
              <>
                <Tab key="home" title="Home" className='ml-7' />
                <Tab key="reviewer" title="Review">
                  <Card className="w-full ">
                    <CardBody>
                      <Review />
                    </CardBody>
                  </Card>
                </Tab>
              </>
            ) : null}
          </Tabs>
        </>
      ) : (
        <>
          {!pathname.includes('myprofile') && !pathname.includes('settings') &&  !pathname.includes('exam') &&<Navbar
            classNames={{
              base: "shadow lg:backdrop-filter-none",
              item: "data-[active=true]:text-primary",
              wrapper: "px-4",
            }}
            height="60px">
            <NavbarBrand>
              <NavbarMenuToggle className="mr-2 h-6 sm:hidden" />
              <p className="font-extrabold text-3xl">OPS</p>
            </NavbarBrand>

            <NavbarContent
              className="ml-4 hidden h-12 w-full max-w-fit gap-6 rounded-full bg-content2 px-4 dark:bg-content1 sm:flex"
              justify="start">
              <NavbarItem>
                <Link aria-current="page" className="flex gap-2 text-center" href={dashboardType ? `/${dashboardType}_dashboard` : `/`}>
                  Home
                </Link>
              </NavbarItem>
            </NavbarContent>
            <NavbarContent justify="end">
              <NavbarItem>
                <Button
                  isIconOnly
                  radius="full"
                  variant="light"
                  onPress={handleThemeChange}>
                  <Icon
                    className="text-default-500"
                    icon={isDarkMode ? "solar:moon-linear" : "solar:sun-linear"}
                    width={24} />
                </Button>
              </NavbarItem>
              <NavbarItem className="hidden lg:flex">
                <Link href="/auth/login">
                  <Button color="primary" variant="shadow">
                    Login
                  </Button>
                </Link>
              </NavbarItem>
            </NavbarContent>
          </Navbar>}
        </>
      )}
    </div>
  );
}
