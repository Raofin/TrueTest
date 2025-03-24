'use client'

import React, { useEffect, useState } from 'react'
import { Avatar, Card, Link } from '@heroui/react'
import '@/app/globals.css'
import { FaLink } from 'react-icons/fa6'
import { usePathname } from 'next/navigation'


export default function ProfilePage() {
  const path=usePathname();
  const [route,setRoute]=useState("");
  useEffect(()=>{
     if(path.startsWith("/profile")) setRoute("profile")
      else setRoute("myprofile")
  },[path])
  return (
    <>
      <div className="h-full flex items-center justify-center mt-5">
        <Card className={`relative rounded-2xl p-8 w-[600px] overflow-visible shadow-none bg-white dark:bg-[#18181b]`}>
          <div className="flex items-center mb-8">
            <Avatar src="" alt="Profile" radius="md" className="absolute w-36 h-36 -mt-20 ml-24" />
            <div className=" ml-64">
              <h2 className="text-3xl font-semibold">username</h2>
              <p className="text-gray-400">@user</p>
            </div>
          </div>

          <p className="text-md mb-4">
            This is my bio section. You can add a short description about yourself here. Lorem ipsum dolor sit amet,
            consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
          </p>

          <hr />
          <div className="space-y-2 mb-4 mt-4">
            <p className="text-md flex gap-3">
              <strong>Email:</strong>
              <a href="">
              <span className='text-[#71717a] dark:text-white'>  useremail@gmail.com</span>
              </a>
            </p>
            <p className="text-md flex gap-3">
              <strong>Institute:</strong> <span className='text-[#71717a] dark:text-white'>Lorem University</span>
            </p>
            <p className="text-md flex gap-3">
              <strong>Phone:</strong> <span className='text-[#71717a] dark:text-white'>+880 123456789</span>
            </p>
            <p className="text-md flex gap-3 items-center">
              <strong>Links:</strong> <span className='flex items-center gap-2 text-[#71717a] dark:text-white'>Portfolio <FaLink /> Github <FaLink /> Linkedin <FaLink /></span>
            </p>

            <p className="text-md flex gap-3">
              <strong>Joined:</strong> <span className='text-[#71717a] dark:text-white'>28 Nov 2026, 10:08 PM</span>
            </p>
          </div>
          <div className="flex justify-center">
            <button className="bg-blue-600 hover:bg-blue-700 font-semibold py-2 px-4 rounded-lg">
            <Link className="text-white" href={`/${route}/edit`}>
                Update Profile
              </Link>
            </button>
          </div>
        </Card>
      </div>
    </>
  )
}
