"use client";

import React from "react";
import { Card, CardHeader, CardBody, Avatar } from "@nextui-org/react";
import { Icon } from "@iconify/react/dist/iconify.js";
import "../../../styles/globals.css";
import { Button, Link } from "@heroui/react";


export default function Page() {

    return (
        <div className="flex h-full w-full items-start justify-center">

            <Card className="my-10 w-[900px] px-20">
                <CardHeader className="relative flex h-[200px] flex-col justify-end overflow-visible bg-gradient-to-br from-pink-300 via-purple-300 to-indigo-400">
                    <Avatar className="h-44 w-44 translate-y-12" src="" />
                </CardHeader>
                <CardBody>
                    <div className="pb-4 pt-6">
                        <div className="ml-72 mb-5">
                            <p className="text-large font-medium ml-5">Tony Reichert</p>
                            <p className="max-w-[90%] text-small text-default-400">tonyreichert@gmail.com</p>
                        </div>
                        <hr />
                        <p className="py-4 text-small text-foreground mb-5">
                            simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.{" "}
                        </p>
                        <hr />
                        <div className="space-y-2 flex items-center">
                            <p className=" font-semibold">Email : </p>
                            <p className="text-sm bg-gray-100 p-1 ml-3">useremail@gmail.com</p>
                        </div>
                        <div className="space-y-2 flex items-center">
                            <p className=" font-semibold">Phone Number : </p>
                            <p className="bg-gray-100 p-1 text-sm ml-3">+880 1526688744</p>
                        </div>
                        <div className="space-y-2 flex items-center">
                            <p className=" font-semibold">Institution : </p>
                            <p className="bg-gray-100 p-1 text-sm ml-3">Lorem ipsum University</p>
                        </div>
                        <div className="flex w-[500px] gap-2 space-y-2 items-center">
                            <div className="flex ">
                                <p className=" font-semibold">Links : </p>
                            </div>
                            <div className="flex items-center">
                                <Icon icon="lucide:link" className="mr-1" /> Portfolio
                            </div>
                            <div className="flex items-center">
                                <Icon icon="lucide:link" className="mr-1" /> Github
                            </div>
                            <div className="flex items-center">
                                <Icon icon="lucide:link" className="mr-1" /> Linkedin
                            </div>
                        </div>
                    </div>
                </CardBody>
                <Button className="mx-44 text-white my-2" color="primary">
                    <Link href="/myprofile/1/update-profile" className="text-white">Update Profile</Link>
                </Button>
            </Card>
        </div>
    );
}