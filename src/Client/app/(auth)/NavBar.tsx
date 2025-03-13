'use client'
import { Avatar, Dropdown, DropdownItem, DropdownMenu, DropdownTrigger, Link } from "@nextui-org/react";
import { FaCircleUser } from "react-icons/fa6";
import { BiSolidLogIn } from "react-icons/bi";
import Logo from '../components/ui/logo/page'
import { Icon } from "@iconify/react/dist/iconify.js";
import ThemeToggle from "../components/ThemeToggle";

export default function Component(){
    
    return(
      <div className=' w-full flex justify-between items-center h-16 px-5 shadow'>
          <Logo />
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
              <ThemeToggle/>
              <button className="flex items-center ">
                  <Dropdown placement="bottom-end">
                      <DropdownTrigger>
                                  <FaCircleUser className='text-gray-400 light:text-white' size={24} />
                      </DropdownTrigger>
                      <DropdownMenu aria-label="Profile Actions" variant="flat">
                          <DropdownItem key="user" className="h-14 gap-2">
                              <div className='flex gap-2 mb-2'>
                                  <Avatar size="md" src="" alt="User Avatar" />
                                  <div>
                                      <p>username</p>
                                      <p>useremail@gmail.com</p>
                                  </div>
                              </div>
                              <hr />
                          </DropdownItem>
  
                          <DropdownItem key="profile" className='text-white'>
                              <Link href={`/profile`} className="flex items-center gap-2">
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
                                  <p>Logout</p>
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