'use client'

import { Button, Input, Link } from '@heroui/react'

export default function Component() {
  return (
    <div className="flex h-screnn w-full items-center justify-center mb-16">
      <div className="flex w-full max-w-sm flex-col mt-16 gap-4 rounded-large px-8 py-5 bg-white dark:bg-[#18181b] ">
        <div className="flex flex-col gap-3">
          <h1 className="text-2xl font-bold text-center mb-6">Reset Password</h1>
        </div>
        <form id="#" className={`flex w-full flex-wrap md:flex-nowrap gap-4 flex-col`}>
          <Input
            isRequired
            label="Email Address"
            name="email"
            type="email"
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            defaultValue={''}
          />
          <Input
            isRequired
            label="New Password"
            name="newpassword"
            type="password"
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
          />
          <Input
            isRequired
            label="Confirm Password"
            name="newconfirmpassword"
            type="password"
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
          />
          <Button className="w-full mt-4 text-medium" color="primary" type="submit">
            Change Password
          </Button>
          <p>
            Want to create a new account?
            <Link className="text-blue-500 ml-2" href="/signup">
              Sign Up
            </Link>
          </p>
        </form>
      </div>
    </div>
  )
}
