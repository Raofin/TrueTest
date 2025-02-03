"use client";

import React from "react";
import {
  Navbar,
  NavbarBrand,
  NavbarContent,
  NavbarItem,
  NavbarMenuToggle,
  Link,
  Button,
  Dropdown,
  DropdownTrigger,
  DropdownMenu,
  DropdownItem,
  Popover,
  PopoverContent,
  PopoverTrigger,
  Avatar,
  Badge,
} from "@nextui-org/react";
import {Icon} from "@iconify/react";

import { useState } from "react";
import NotificationsCard from "./notifications-card";

export default function Component() {
  const [page, setPage] = useState("home");
  return (
    <Navbar
      classNames={{
        base: "lg:bg-transparent lg:backdrop-filter-none",
        item: "data-[active=true]:text-primary",
        wrapper: "px-4 sm:px-6",
      }}
      height="60px">
      <NavbarBrand>
        <NavbarMenuToggle className="mr-2 h-6 sm:hidden" />
        <p className="font-extrabold text-3xl ">OPS</p>
      </NavbarBrand>
      <NavbarContent
        className="ml-4 hidden h-12 w-full max-w-fit gap-4 rounded-full bg-content2 px-4 dark:bg-content1 sm:flex"
        justify="start">
        <NavbarItem isActive={page === "home"}>
          <Link className="flex gap-2 text-inherit" href="/" >
            Home
          </Link>
        </NavbarItem>
        <NavbarItem isActive={page === "attend-exam"}>
          <Link aria-current="page" className="flex gap-2 text-inherit" href="/candidate_dashboard/attend-exam" onPress={() => setPage("attend-exam")}>
            Attend Exam
          </Link>
        </NavbarItem>
        <NavbarItem>
          <Link className="flex gap-2 text-inherit" href="/candidate_dashboard/exam-schedule">
            Exam Schedule
          </Link>
        </NavbarItem>
        <NavbarItem>
          <Link className="flex gap-2 text-inherit" href="/candidate_dashboard/view-result">
            View Result
          </Link>
        </NavbarItem>
      </NavbarContent>
      <NavbarContent
        className="ml-auto flex h-12 max-w-fit items-center gap-0 rounded-full p-0 lg:bg-content2 lg:px-1 lg:dark:bg-content1"
        justify="end">
        <NavbarItem className="hidden sm:flex">
          <Button isIconOnly radius="full" variant="light">
            <Icon className="text-default-500" icon="solar:sun-linear" width={24} />
          </Button>
        </NavbarItem>
        <NavbarItem className="hidden sm:flex">
           <Button isIconOnly radius="full" variant="light">
             <Link href="/candidate_dashboard/settings"> <Icon className="text-default-500" icon="solar:settings-linear" width={24} />
             </Link> </Button>

        </NavbarItem>
        <NavbarItem className="flex">
          <Popover offset={12} placement="bottom-end">
            <PopoverTrigger>
              <Button
                disableRipple
                isIconOnly
                className="overflow-visible"
                radius="full"
                variant="light">
                <Badge color="danger" content="5" showOutline={false} size="md">
                  <Icon className="text-default-500" icon="solar:bell-linear" width={22} />
                </Badge>
              </Button>
            </PopoverTrigger>
            <PopoverContent className="max-w-[90vw] p-0 sm:max-w-[380px]">
              <NotificationsCard className="w-full shadow-none" />
            </PopoverContent>
          </Popover>
        </NavbarItem>
        <NavbarItem className="px-2">
          <Dropdown placement="bottom-end">
            <DropdownTrigger>
              <button className="mt-1 h-8 w-8 outline-none transition-transform">
                <Badge
                  className="border-transparent"
                  color="success"
                  content=""
                  placement="bottom-right"
                  shape="circle"
                  size="sm">
                  <Avatar size="sm" src="https://i.pravatar.cc/150?u=a04258114e29526708c" />
                </Badge>
              </button>
            </DropdownTrigger>
            <DropdownMenu aria-label="Profile Actions" variant="flat">
              <DropdownItem key="profile" className="h-14 gap-2">
                <p className="font-semibold">Signed in as</p>
                <p className="font-semibold">johndoe@example.com</p>
              </DropdownItem>
              <DropdownItem key="settings"><Link href="/candidate_dashboard/myprofile">My Profile</Link></DropdownItem>
              <DropdownItem key="logout" color="danger">
               <Link href="/"> Log Out</Link>
              </DropdownItem>
            </DropdownMenu>
          </Dropdown>
        </NavbarItem>
      </NavbarContent>
    </Navbar>
  );
}
