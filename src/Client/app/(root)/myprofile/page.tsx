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
      <div className="min-h-screen flex items-center justify-center mt-5">
        <Card className={`rounded-2xl shadow-lg p-8 w-[600px] `}>
          <div className="flex items-center justify-center gap-2 mb-8">
            <Avatar src="" alt="Profile" radius="md" className="w-36 h-36 object-cover " />
            <div className="">
              <h2 className="text-2xl font-semibold">username</h2>
              <p className="text-gray-400">useremail@gmail.com</p>
            </div>
          </div>

          <p className="text-sm mb-4">
            This is my bio section. You can add a short description about yourself here. Lorem ipsum dolor sit amet,
            consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
          </p>

          <hr />
          <div className="space-y-2 mb-4 mt-4">
            <p className="text-sm">
              <strong>Email:</strong>
              <a href="" className="text-blue-400">
                useremail@gmail.com
              </a>
            </p>
            <p className="text-sm">
              <strong>Institute:</strong> Lorem University
            </p>
            <p className="text-sm">
              <strong>Phone:</strong> +880 123456789
            </p>
            <p className="text-sm flex gap-2 items-center">
              <strong>Links:</strong>Portfolio <FaLink /> Github <FaLink /> Linkedin <FaLink />
            </p>

            <p className="text-sm">
              <strong>Joined:</strong> 28 Nov 2026, 10:08 PM
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
