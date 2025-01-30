"use client";

import * as React from "react";
import {Card, CardBody} from "@nextui-org/card";
import {Avatar} from "@nextui-org/avatar";

import {Button, Badge, Spacer} from "@nextui-org/react";

interface ProfileSettingCardProps {
    className?: string;
}

const Page = React.forwardRef<HTMLDivElement, ProfileSettingCardProps>(
    ({className, ...props}, ref) => (
        <div ref={ref} className="m-5">
            <div>
                <p className="text-base font-medium text-default-700">Profile</p>
                <Card className="mt-4 bg-default-100" shadow="none">
                    <CardBody>
                        <div className="flex items-center gap-4">
                            <Badge classNames={{ badge: "w-5 h-5" }}
                                content={
                                    <Button
                                        isIconOnly
                                        className="h-5 w-5 min-w-5 bg-background p-0 text-default-500"
                                        radius="full"
                                        size="sm"
                                        variant="bordered">
                                    </Button>
                                }
                                placement="bottom-right"
                                shape="circle">
                                <Avatar
                                    className="h-16 w-16"
                                    src="https://nextuipro.nyc3.cdn.digitaloceanspaces.com/components-images/avatars/e1b8ec120710c09589a12c0004f85825.jpg"
                                />
                            </Badge>
                            <div>
                                <p className="text-sm font-medium text-default-600">Kate Moore</p>
                                <p className="mt-1 text-xs text-default-400">kate.moore@acme.com</p>
                                <p className="text-xs text-default-400">01875555787</p>
                            </div>
                        </div>
                    </CardBody>
                </Card>
            </div>
            {/*<Spacer y={4} />*/}
          <Card className="mt-4 bg-default-100" shadow="none">
              <CardBody>
                  <div>
                      <p className="text-base font-medium text-default-700">Title</p>
                      <p className="mt-1 text-sm font-normal text-default-400">Student</p>
                  </div>
                  <Spacer y={2} />
                  {/* Location */}
                  <div>
                      <p className="text-base font-medium text-default-700">Location</p>
                      <p className="mt-1 text-sm font-normal text-default-400">Dhaka,Bangladesh</p>

                  </div>
                  <Spacer y={4} />
                  <div>
                      <p className="text-base font-medium text-default-700">Biography</p>
                      <p className="mt-1 text-sm font-normal text-default-400">
                          Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage,
                          and going through the cites of the word in classical literature, discovered the undoubtable source.
                      </p>
                  </div>
              </CardBody>
          </Card>
        </div>
    ),
);

Page.displayName = "Page";

export default Page;
