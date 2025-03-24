'use client'

import { Button, Card, Link } from '@heroui/react'
import { usePathname } from 'next/navigation'
import { useEffect, useState } from 'react'

export default function Component() {
    const path=usePathname();
    const [route,setRoute]=useState("");
    useEffect(()=>{
       if(path.startsWith("/settings")) setRoute("settings")
        else setRoute("mysettings")
    },[path])
  return (
    <>

      <div className="mt-32 flex items-center justify-center ">
        <Card className={`p-8 rounded-lg max-w-md w-full `}>
          <h1 className="text-2xl font-semibold mb-6 text-center">Account settings</h1>
          <hr />
          <div className="flex flex-col space-y-2">
            <div className="mt-2 flex items-center">
              <p className=" font-semibold">Username : </p>
              <p className="text-sm ml-3">user-name</p>
            </div>

            <div className="flex items-center">
              <p className=" font-semibold">Email : </p>
              <p className="text-sm ml-3">useremail@gmail.com</p>
            </div>
            <div className="flex items-center">
              <p className=" font-semibold">Password : </p>
              <p className="text-sm ml-3">**********</p>
            </div>
            <div className="flex items-center">
              <p className=" font-semibold">Joined : </p>
              <p className="text-sm ml-3">21 Nov 2024, 10:00PM</p>
            </div>
          </div>
          <div className="mt-5 flex w-full justify-center">
            <Button color="primary">
              <Link className="text-white" href={`/${route ? route : 'mysettings'}/edit`}>
                Change Settings
              </Link>
            </Button>
          </div>
        </Card>
      </div>
    </>
  )
}
