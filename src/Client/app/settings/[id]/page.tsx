"use client";
import React, { useState } from "react";
import '../../../styles/globals.css'

import { Button ,Link} from "@heroui/react";

const Component: React.FC = () => {

  return (
    <div className="flex dark:bg-gray-900 dark:text-white items-center justify-center h-[600px] bg-gray-100">
      <form className=" p-8 rounded-lg shadow-lg max-w-md w-full">
        <h1 className="text-2xl font-semibold mb-6 text-center">
          Account Settings
        </h1>
        <hr/>
        <div>
          <div className="space-y-2 mt-2 flex items-center">
            <p className=" font-semibold">Username : </p>
            <p className="text-sm bg-gray-100 p-1 ml-3">user-name</p>
          </div>

          <div className="space-y-2 flex items-center">
            <p className=" font-semibold">Email : </p>
            <p className="text-sm bg-gray-100 p-1 ml-3">useremail@gmail.com</p>
          </div>
          <div className="space-y-2 flex items-center">
            <p className=" font-semibold">Password : </p>
            <p className="text-sm bg-gray-100 p-1 ml-3">**********</p>
          </div>
          <div className="space-y-2 flex items-center">
            <p className=" font-semibold">Joined : </p>
            <p className="text-sm bg-gray-100 p-1 ml-3">21 Nov 2024, 10:00PM</p>
          </div>
        </div>
        <Button className="ml-32 mt-5" color="primary"><Link className="text-white" href='/settings/1/change-settings'>Change Settings</Link></Button>
      </form>
    </div>
  );
};
export default Component;