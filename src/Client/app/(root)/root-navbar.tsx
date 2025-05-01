"use client";

import React from "react";
import "@/app/globals.css";
import NavBar from "@/components/NavBar";
import Logo from "@/components/ui/TrueTestLogo";
import { Link, Navbar, NavbarContent, NavbarItem } from "@heroui/react";
import { useRouter } from "next/navigation";
import { useAuth } from '@/context/AuthProvider'

export default function RootNavBar() {
    const router = useRouter();
    const {user}=useAuth()
    return (
        <div className="flex w-full justify-between items-center bg-[#eeeef0] dark:bg-[#000000] h-16 px-14 pt-3">
           <Link href="/home"> <div className="bg-[#eeeef0] dark:bg-[#000000] ">
                <Logo />
            </div></Link>
            <div className="flex">
                <Navbar
                    classNames={{
                        wrapper: " justify-end bg-[#eeeef0] dark:bg-[#000000]",
                        item: "hidden",
                    }} >
                    <NavbarContent
                        className="h-11 gap-5 rounded-full bg-[#ffffff] px-4 dark:bg-[#18181b]"
                        justify="end"
                    >
                        <NavbarItem
                            style={{ cursor: "pointer" }}
                            onClick={() => router.push("/home")}
                        >
                            Home
                        </NavbarItem>
                        <NavbarItem
                            style={{ cursor: "pointer" }}
                            onClick={() => router.push("/my-exams")}
                        >
                            My Exams
                        </NavbarItem>
                        {
                            user?.roles.includes("Admin") && <NavbarItem
                            style={{ cursor: "pointer" }}
                            onClick={() => router.push("/overview")} >
                            Admin Panel
                        </NavbarItem>
                        }
                    </NavbarContent>
                </Navbar>
                <NavBar />
            </div>
        </div>
    );
}
