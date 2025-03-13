"use client";
import React from "react";
import '../../styles/globals.css'

import { Button ,Link} from "@heroui/react";
import { useParams } from "next/navigation";
import NavBar from '../navigation-header/NavBar'
const Component: React.FC = () => {
  // const params = useParams();
  // const { id } = params;
  const Mode=localStorage.getItem("theme")
  return (
    <>
    <NavBar/>
    <div className="my-auto flex flex-col items-center justify-center h-screen">
      <form className={`p-8 rounded-lg shadow-lg max-w-md w-full ${Mode==='dark'?"bg-[#18181b]":"bg-white"}`}>
        <h1 className="text-2xl font-semibold mb-6 text-center">
          Account settings 
        </h1>
        <hr/>
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
        <Button className="ml-32 mt-5" color="primary"><Link className="text-white" href='/settings/edit'>Change Settings</Link></Button>
      </form>
    </div>
    </>
  );
};
export default Component;