"use client"
import { Link, Button, Navbar, NavbarBrand, NavbarContent, NavbarItem, NavbarMenuToggle } from '@heroui/react'
import '../styles/globals.css'
import { useEffect, useState } from 'react';
import Login from './(auth)/login/page'
import Logo from './components/ui/logo/page'
import { FaCircleUser } from "react-icons/fa6";
import { BiSolidLogIn } from "react-icons/bi";
import { FaSun } from "react-icons/fa6";
import { IoMoon } from "react-icons/io5";

export default function Component() {
  const [isDarkMode, setIsDarkMode] = useState(false);
  useEffect(() => {
    const savedTheme = localStorage.getItem("theme") || "light";
    setIsDarkMode(savedTheme === "dark");
    document.documentElement.classList.toggle("dark", savedTheme === "dark");
  }, []);

  const handleThemeChange = () => {
    const newTheme = isDarkMode ? "light" : "dark";
    setIsDarkMode(!isDarkMode);
    localStorage.setItem("theme", newTheme);

    document.documentElement.classList.toggle("dark", newTheme === "dark");
  };

  return (
    <>
    <div className=' w-full flex justify-between items-center h-16 px-5 '>
          <div className="flex items-center gap-2">
            <Logo />
            <p className="font-extrabold text-xl">
              <span className="text-red-500">True</span>
              <span className="text-blue-500">Test</span>
            </p>
          </div>
        <div className="flex items-end gap-4 flex-1 justify-end  "> 
          <div className=' flex items-center gap-4'> 
            <div>
              <Link className='text-gray-400 light:text-black' href="/">About Us</Link>
            </div>
            <div>
              <Link className='text-gray-400 light:text-black' href="/">Contact</Link>
            </div>
            <div>
              <Link className='text-gray-400 light:text-black' href="/">Support</Link>
            </div>
            <div>
              <Link className='text-gray-400 light:text-black' href="/">FAQs</Link>
            </div>
          </div>
          <div className='flex items-center gap-3 ml-5'> 
            <div onClick={handleThemeChange} className='text-gray-400 light:text-black'>
              {isDarkMode ? <IoMoon width={32}/> :<FaSun width={32}/>} 
            </div>
            <div> <FaCircleUser className='text-gray-400 light:text-white' size={24} /></div>
            <div> <BiSolidLogIn className='text-gray-400 light:text-white' size={24}/></div>
          </div>
        </div>
    </div>
      <div className='flex justify-around items-center gap-12 shadow-xl'>
        <div className="flex flex-col items-center gap-4 flex-1">
          <div className="flex items-center gap-2">
            <Logo />
            <p className="font-extrabold text-3xl">
              <span className="text-red-500">True</span>
              <span className="text-blue-500">Test</span>
            </p>
          </div>
          <div className='flex flex-col items-center justify-center text-5xl font-bold '>
            <p> Your Secure Platform</p>
            <p>for Assessments</p>
          </div>
          <p className="text-[#71717A] light:text-black max-w-md text-center">
            TrueTest ensures a fair, secure environment with real-time proctoring and anti-cheating features, so you can focus on showcasing your skills!
          </p>
        </div>
        <div className='flex-1'>
          <Login />
        </div>
      </div>
    
    </>
  );
}
