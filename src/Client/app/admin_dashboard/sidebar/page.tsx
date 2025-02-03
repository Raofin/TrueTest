"use client";

import React from "react";
import {Avatar, Button, ScrollShadow, Spacer} from "@nextui-org/react";
import {Icon} from "@iconify/react";

import {items} from "./sidebar-items";

import Sidebar from "./sidebar";

export default function Component() {
  return (
    <div>
      <h2 className="text-2xl ml-8 font-bold">OPS</h2>
      <div className="relative flex h-full w-72 flex-1 flex-col border-r-small border-divider p-6">
        <Spacer y={3} />
        <div className="flex items-center gap-3 px-4">
          <Avatar isBordered size="sm" src="https://i.pravatar.cc/150?u=a04258114e29026708c" />
          <div className="flex flex-col">
            <p className="text-small font-medium text-default-600">John Doe</p>
            <p className="text-tiny text-default-400">Software Developer</p>
          </div>
        </div>
        <ScrollShadow className="-mr-6 py-6 pr-6">
          <Sidebar defaultSelectedKey="home" items={items} />
         <hr className="my-4"/>
        <div className="flex flex-col">
          <Button fullWidth className="justify-start text-default-500 data-[hover=true]:text-foreground"
            startContent={
              <Icon className="text-default-500" icon="solar:info-circle-line-duotone" width={24} />}
            variant="light">
            Help & Information
          </Button>
          <Button
            className="justify-start text-default-500 data-[hover=true]:text-foreground"
            startContent={
              <Icon
                className="rotate-180 text-default-500"
                icon="solar:minus-circle-line-duotone"
                width={24}
              />
            }
            variant="light">
            Log Out
          </Button>
        </div>
        </ScrollShadow>
      </div>
    </div>
  );
}
