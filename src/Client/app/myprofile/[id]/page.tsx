"use client";

import React from "react";

import {Card, CardHeader, CardBody, Button, Avatar,Chip} from "@nextui-org/react";
import '../../../styles/globals.css'
import { SocialIcon } from 'react-social-icons'
export default function Page({ params }: { params: Promise<{ id: string }> }) {
{
    const paramsId=React.use(params);
   return(
       <div className="flex h-full  w-full items-start justify-center">
           <h3>Profile : {paramsId.id}</h3>
           <Card className="my-10 w-[900px]">
               <CardHeader className="relative flex h-[200px] flex-col justify-end overflow-visible bg-gradient-to-br from-pink-300 via-purple-300 to-indigo-400">
                   <Avatar className="h-44 w-44 translate-y-12" src="https://i.pravatar.cc/150?u=a04258114e29026708c"/>
                   <Button
                       className="absolute right-3 top-3 bg-white/20 text-white dark:bg-black/20" radius="full" size="sm" variant="light">
                       Edit Profile
                   </Button>
               </CardHeader>
               <CardBody>
                   <div className="pb-4 pt-6">
                       <p className="text-large font-medium">Tony Reichert</p>
                       <p className="max-w-[90%] text-small text-default-400">tonyreichert@gmail.com</p>
                       <div className="flex gap-2 pb-1 pt-2">
                           <Chip variant="flat">Design</Chip>
                           <Chip variant="flat">UI/UX</Chip>
                           <Chip variant="flat">Photography</Chip>
                       </div>
                       <p className="py-4 text-small text-foreground">
                           Creator of Radify Icons Set. 500+ icons in 6 styles, SVG and Figma files, and more.
                       </p>

                         <div className='space-y-1 p-2'>
                             <h3 className='text-lg font-semibold'>Location  </h3>
                             <p className="text-sm bg-gray-100 p-1 ml-3">California</p>
                         </div>
                         <div className='space-y-1 p-2'>
                           <h3 className='text-lg font-semibold'>Phone Number </h3>
                           <p className="bg-gray-100 p-1 text-sm ml-3">01526688744</p>
                        </div>
                        <div className='space-y-1 p-2'>
                           <h3 className='text-lg font-semibold'>Institution  </h3>
                           <p className="bg-gray-100 p-1 text-sm ml-3">Lorem ipsum University</p>
                         </div>
                          <div className='space-y-1 p-2'>
                              <h3 className='text-lg font-semibold'>Social Account  </h3>
                              <p className='space-x-2'><SocialIcon url="https://twitter.com" />
                                  <SocialIcon  url="https://facebook.com"/>
                                  <SocialIcon  url="https://linkedin.com" /></p>
                          </div>

                   </div>
               </CardBody>
           </Card>
       </div>
   )
}}
