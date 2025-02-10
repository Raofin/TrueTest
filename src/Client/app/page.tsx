"use client"
import '../styles/globals.css'
import {Navbar,NavbarBrand,NavbarContent,NavbarItem,Link,Button} from "@nextui-org/react";

export default function Component(){
    return(
        <>
            <div>
                <Navbar isBordered>
                    <NavbarBrand>
                        <p className="font-extrabold text-3xl text-inherit">OPS</p>
                    </NavbarBrand>
                    <NavbarContent className="hidden sm:flex gap-8" justify="center">
                        <NavbarItem>
                            <Link href="/">Home</Link>
                        </NavbarItem>
                    </NavbarContent>
                    <NavbarContent justify="end">
                        <NavbarItem className="hidden lg:flex">
                            <Link href="/login"><Button color="primary" variant="shadow">Login</Button></Link>
                        </NavbarItem>
                    </NavbarContent>
                </Navbar>
            </div>
            <div className="flex justify-center items-center w-full mt-24 text-xl">Welcome to OPS</div>
        </>

    )
}