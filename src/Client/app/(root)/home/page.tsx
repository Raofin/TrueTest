'use client'

import { Card, CardHeader, CardBody, Button } from '@heroui/react'
import { Icon } from '@iconify/react/dist/iconify.js'
import { useState } from 'react'
import RootNavBar from '../root-navbar'

const stats = [
  {id:1, icon: 'fa-solid:camera', title: 'Camera' },
  {id:2, icon: 'fa-solid:microphone-alt', title: 'Microphone' },
  {id:3, icon: 'fa-solid:desktop', title: 'Screen Recording' }, 
  {id:4, icon: 'fa-solid:clipboard-list', title: 'Clipboard' },
];

export default function Component() {
  const [permit, setPermit] = useState(false)
  return (
    <>
    <RootNavBar/>
    <div className="flex flex-col gap-6 items-center justify-center text-center h-full w-full ">
      <div className="gap-5 flex">
        {stats.map((stat) => (
          <Card key={stat.id} className="py-4 w-[200px] text-center flex flex-col items-center shadow-none bg-white dark:bg-[#18181b]">
            <CardHeader className="pb-0 pt-2 px-4 flex flex-col items-center">
              <Icon icon={stat.icon} width={50} height={50} />
            </CardHeader>
            <CardBody className="py-2 mt-4 text-center">
              <h2 className="text-lg font-semibold">{stat.title}</h2>
            </CardBody>
          </Card>
        ))}
      </div>
      <p className="w-[800px] text-center mt-5">
        contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin
        literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney
        College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and
        going through the cites of the word in classical literature, discovered the
      </p>
      <div>
        <Button
          color="primary"
          className="my-10 w-[200px] ml-10"
          onPress={() => setPermit(!permit)}
          isDisabled={permit}
        >
          {permit ? 'Permission Granted' : 'Grant Permission'}
        </Button>
      </div>
    </div>
    </>
  )
}
