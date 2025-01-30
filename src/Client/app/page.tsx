"use client"
import {Navbar, NavbarBrand, NavbarContent, NavbarItem, Link, Button} from "@heroui/react";


export default function App() {
    return (
        <Navbar isBordered>
            <NavbarBrand>
                <div>
                    <p className="font-extrabold text-3xl text-inherit">OPS</p>
                </div>
            </NavbarBrand>
            <NavbarContent className="hidden sm:flex gap-8" justify="center">
                <NavbarItem isActive>
                    <Link aria-current="page"  href="#">
                        Home
                    </Link>
                </NavbarItem>
                <NavbarItem >
                    <Link color="foreground" href="#">
                        Attend Exam
                    </Link>
                </NavbarItem>
                <NavbarItem>
                    <Link color="foreground" href="#">
                        Exam Schedule
                    </Link>
                </NavbarItem>
                <NavbarItem>
                    <Link color="foreground" href="#">
                        View Result
                    </Link>
                </NavbarItem>
            </NavbarContent>
            <NavbarContent justify="end">
                <NavbarItem className="hidden lg:flex">
                    <Link href="/login"><button className="bg-blue-600 text-white py-1 rounded-xl px-2">Login</button></Link>
                </NavbarItem>

            </NavbarContent>
        </Navbar>
    );
}
