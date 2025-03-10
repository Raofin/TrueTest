'use client'
import { Avatar, Badge, Dropdown, DropdownItem, DropdownMenu, DropdownTrigger, Link } from "@nextui-org/react";
import { FaCircleUser } from "react-icons/fa6";
import { BiSolidLogIn } from "react-icons/bi";
import { FaSun } from "react-icons/fa6";
import { IoMoon } from "react-icons/io5";
import Logo from '../components/ui/logo/page'
import { useEffect, useState } from "react";
import { Icon } from "@iconify/react/dist/iconify.js";
import { useAuth } from "../context/AuthProvider";

export default function Component(){
      const [isDarkMode, setIsDarkMode] = useState(false);
      useEffect(() => {
        const savedTheme = localStorage.getItem("theme") || "light";
        setIsDarkMode(savedTheme === "dark");
        document.documentElement.classList.toggle("dark", savedTheme === "dark");
      }, []);
    const {user,handleLogout}=useAuth();
      const handleThemeChange = () => {
        const newTheme = isDarkMode ? "light" : "dark";
        setIsDarkMode(!isDarkMode);
        localStorage.setItem("theme", newTheme);
    
        document.documentElement.classList.toggle("dark", newTheme === "dark");
      };
    return(
      <div className=' w-full flex justify-between items-center h-16 px-5'>
      <div className="flex items-center gap-2">
          <Logo />
          <p className="font-extrabold text-xl">
              <span className="text-red-500">True</span>
              <span className="text-blue-500">Test</span>
          </p>
      </div>
      <div className="flex items-center gap-4 flex-1 justify-end "> 
          <div className=' flex items-center gap-4 mr-5'>
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
                  {isDarkMode ? <IoMoon className="w-5 h-5" /> : <FaSun className="w-5 h-5"/>}
              </div>
              <button className="flex items-center ">
                  <Dropdown placement="bottom-end">
                      <DropdownTrigger>
                          {/* <div className="flex items-center w-7 outline-none transition-transform"> */}
                              {/* <Badge
                                  className="border-transparent"
                                  color="success"
                                  content=""
                                  placement="bottom-right"
                                  shape="circle"
                                  size="sm"> */}
                                  <FaCircleUser className='text-gray-400 light:text-white' size={24} />
                              {/* </Badge> */}
                          {/* </div> */}
                      </DropdownTrigger>
                      <DropdownMenu aria-label="Profile Actions" variant="flat">
                          <DropdownItem key="user" className="h-14 gap-2">
                              <div className='flex gap-2 mb-2'>
                                  <Avatar size="md" src="" alt="User Avatar" />
                                  <div>
                                      <p>{user?.username}</p>
                                      <p>{user?.email}</p>
                                  </div>
                              </div>
                              <hr />
                          </DropdownItem>
  
                          <DropdownItem key="profile" className='text-white'>
                              <Link href={`/myprofile/${user?.accountId}`} className="flex items-center gap-2">
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
                              <Link href="/login" className="flex items-center gap-2">
                                  <Icon icon="lucide:log-out" className="w-5 h-5" />
                                  <p onClick={handleLogout}>Logout</p>
                              </Link>
                          </DropdownItem>
                      </DropdownMenu>
                  </Dropdown>
              </button>
              <div> <BiSolidLogIn className='text-gray-400 light:text-white' size={24} /></div>
          </div>
      </div>
  </div>
    )
}