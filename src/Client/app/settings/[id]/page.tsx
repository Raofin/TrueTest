"use client";

import React from "react";
import {Card, CardHeader, CardBody, Button, Avatar, Badge, Input, CardFooter} from "@nextui-org/react";
import {Icon} from "@iconify/react";
import '../../../styles/globals.css'

export default function Page({ params }: { params: Promise<{ id: string }> }) {
        const paramsId=React.use(params);
        return(
        <Card className="w-[900px] ml-44 p-2">
            <CardHeader className="flex flex-col items-start px-4 pb-0 pt-4">
                <p className="text-sm">Account Details :  {paramsId.id}</p>
                <div className="flex gap-4 py-4">
                    <Badge
                        classNames={{
                            badge: "w-5 h-5",
                        }}
                        color="primary"
                        content={
                            <Button
                                isIconOnly
                                className="p-0 text-primary-foreground"
                                radius="full"
                                size="sm"
                                variant="light"
                            >
                                <Icon icon="solar:pen-2-linear" />
                            </Button>
                        }
                        placement="bottom-right"
                        shape="circle"
                    >
                        <Avatar className="h-14 w-14" src="https://i.pravatar.cc/150?u=a04258114e29026708c" />
                    </Badge>
                    <div className="flex flex-col items-start justify-center">
                        <p className="font-medium">Tony Reichert</p>
                        <span className="text-small text-default-500">Professional Designer</span>
                    </div>
                </div>
                <p className="text-small text-default-400">
                    The photo will be used for your profile, and will be visible to other users of the
                    platform.
                </p>
            </CardHeader>
            <CardBody className="grid grid-cols-1 gap-4 md:grid-cols-2">
                {/* Username */}
                <Input label="Username" labelPlacement="outside" placeholder="Enter username" />
                {/* Email */}
                <Input label="Email" labelPlacement="outside" placeholder="Enter email" />
                {/* First Name */}
                <Input label="First Name" labelPlacement="outside" placeholder="Enter first name" />
                {/* Last Name */}
                <Input label="Last Name" labelPlacement="outside" placeholder="Enter last name" />
                {/* Phone Number */}
                <Input label="Phone Number" labelPlacement="outside" placeholder="Enter phone number" />
                {/* Country */}
                <Input label="Country" labelPlacement="outside" placeholder="Enter Country" />
                {/* Address */}
                <Input label="Address" labelPlacement="outside" placeholder="Enter address" />

            </CardBody>

            <CardFooter className="mt-4 justify-end gap-2">
                <Button radius="full" variant="bordered">
                    Cancel
                </Button>
                <Button color="primary" radius="full">
                    Save Changes
                </Button>
            </CardFooter>
        </Card>
    );
}
