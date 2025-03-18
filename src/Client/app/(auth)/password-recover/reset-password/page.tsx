'use client'

import useTheme from '@/app/hooks/useTheme'
import { Button, Input, Link } from '@heroui/react'
import { useSearchParams } from 'next/navigation'

export default function Component() {
  const searchParams = useSearchParams()
  const email = searchParams.get('email')
  const Mode = useTheme()

  return (
    <div className="flex h-full w-full items-center justify-center mb-16">
      <div className="flex w-full max-w-sm flex-col mt-16 gap-4 rounded-large bg-content1 px-8 pb-10 pt-6 shadow-small">
        <div className="flex flex-col gap-3">
          <h1 className="text-xl font-semibold text-center mb-6">Reset Password</h1>
        </div>
        <form
          id="#"
          className={`flex w-full flex-wrap md:flex-nowrap gap-4 flex-col ${
            Mode === 'dark' ? 'bg-[#18181b]' : 'bg-white'
          }`}
        >
          <Input
            isRequired
            label="Email Address"
            name="email"
            type="email"
            variant="bordered"
            defaultValue={email || ''}
          />
          <Input isRequired label="New Password" name="newpassword" type="password" variant="bordered" />
          <Input isRequired label="Confirm Password" name="newconfirmpassword" type="password" variant="bordered" />
          <Button className="w-full mt-4 text-medium" color="primary" type="submit">
            Change Password
          </Button>
          <p>
            Want to create a new account?{' '}
            <Link className="text-blue-500 ml-2" href="/signup">
              Sign Up
            </Link>
          </p>
        </form>
      </div>
    </div>
  )
}
