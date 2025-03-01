"use client";

import React, { useState } from "react";
import {
  Card,
  CardHeader,
  CardBody,
  Button,Link,
  Avatar,
  Input,
  Textarea,
  CardFooter,
} from "@nextui-org/react";
import { Icon } from "@iconify/react";
import "../../../../styles/globals.css";

export default function ProfileDetails() {
    const [socialLinks, setSocialLinks] = useState([""]);

  const handleAddSocialLink = () => {
    setSocialLinks([...socialLinks, ""]);
  };

  const handleSocialLinkChange = (index: number, value: string) => {
    const newLinks = [...socialLinks];
    newLinks[index] = value;
    setSocialLinks(newLinks);
  };
  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <Card className="w-[600px] p-6 shadow-lg rounded-lg bg-white">
        <CardHeader className="flex flex-col items-center text-center">
          <div className="relative">
            <Avatar className="h-24 w-24" size="lg" src="" />
            <button className="absolute bottom-2 right-2 bg-white p-1 rounded-full shadow">
              <Icon icon="solar:pen-2-linear" width={20} />
            </button>
          </div>
          <h2 className="text-lg font-semibold mt-2">Update Profile</h2>
        </CardHeader>
        <CardBody className="grid gap-4">
          <div className="grid grid-cols-2 gap-4">
            <Input label="" placeholder="Enter first name" />
            <Input label="" placeholder="Enter last name" />
          </div>
          <Textarea label="" placeholder="Write your bio here." />
          <Input label="" placeholder="Enter institute name" />
          <Input label="" placeholder="Enter phone number" />
          <Input label="" placeholder="Enter social links" />
          <div className="flex flex-col gap-2">
          {socialLinks.map((link, index) => (
            <div className="flex items-center justify-center" key={index}>
            <Input
              key={index}
              label={`Social Link ${index + 1}`}
              labelPlacement="outside"
              placeholder="Enter social link"
              value={link}
              onChange={(e) => handleSocialLinkChange(index, e.target.value)}
            />
       
            <Icon onClick={handleAddSocialLink} icon="lucide:circle-plus" className="mr-2 w-7 h-5 mt-5 pointer" />
        
          </div>
          ))}
        </div>
      </CardBody>

        <CardFooter className="flex justify-center mt-4">
          <Button color="primary" radius="full">
           <Link className="text-white" href="/myprofile/1"> Save Changes</Link>
          </Button>
        </CardFooter>
      </Card>
    </div>
  );
}
