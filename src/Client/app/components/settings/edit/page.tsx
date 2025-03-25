'use client'

import React from 'react'
import { Button, Input, Form, Card } from '@heroui/react'
import '@/app/globals.css'

export default function Component() {
  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
  }
  return (
    <>
      <div className="flex mt-24 w-full items-center justify-center ">
        <Card className="flex w-full max-w-sm flex-col gap-4 rounded-large shadow-none bg-white dark:bg-[#18181b] bg-content1 px-8 pb-10 pt-6">
          <div className="flex flex-col gap-3">
            <h1 className="text-2xl font-semibold text-center mb-4"> Account Settings</h1>
          </div>
          <Form
            className="flex w-full flex-wrap md:flex-nowrap gap-4 flex-col"
            validationBehavior="native"
            onSubmit={handleSubmit}
          >
            <Input isRequired label="Username" name="email" type="email" className='bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl' />
            <Input isRequired label="Current Password" name="newpassword" type="password" className='bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl' />
            <Input isRequired label="New Password" name="newpassword" type="password" className='bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl'/>
            <Input isRequired label="Confirm Password" name="newconfirmpassword" type="password"  className='bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl'/>
            <Button className="w-full mt-2 text-medium" color="primary" type="submit">
              Save Changes
            </Button>
          </Form>
        </Card>
      </div>
    </>
  )
}
