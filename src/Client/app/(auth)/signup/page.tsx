'use client'

import React, { useState } from 'react'
import { Button, Input, Checkbox, Link, Divider, Card } from '@heroui/react'
import { Icon } from '@iconify/react'
import '@/app/globals.css'

export default function Signup() {
  const [isVisible] = useState<boolean>(false)
  const [isConfirmVisible] = useState<boolean>(false)
  const [loading] = useState<boolean>(false)
  return (
    <div>
      <div className="container mx-auto px-4 py-12 ">
        <Card className={`max-w-sm mx-auto  rounded-xl px-8 py-5`}>
          <h2 className="text-2xl font-bold text-center my-4 ">Sign Up</h2>
          <form id="#" className="flex flex-col gap-4">
            <Input
              isRequired
              label="Username"
              name="username"
              type="text"
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            />
            <div className="relative">
              <Input
                isRequired
                label="Email"
                name="email"
                type="email"
                className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              />
            </div>
            <Input
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              isRequired
              endContent={
                <button type="button">
                  <Icon
                    className="text-2xl text-default-400"
                    icon={isVisible ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                  />
                </button>
              }
              label="Password"
              name="password"
              type={isVisible ? 'text' : 'password'}
            />
            <Input
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              isRequired
              endContent={
                <button type="button">
                  <Icon
                    className="text-2xl text-default-400"
                    icon={isConfirmVisible ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                  />
                </button>
              }
              label="Confirm Password"
              name="confirmPassword"
              type={isConfirmVisible ? 'text' : 'password'}
            />
            <Checkbox isRequired className="py-4" size="sm" name="agreeTerms">
              I agree with the &nbsp;
              <Link href="#" size="sm">
                Terms
              </Link>
              &nbsp; and &nbsp;
              <Link href="#" size="sm">
                Privacy Policy
              </Link>
            </Checkbox>
            <Button color="primary" type="submit" isDisabled={loading}>
              {loading ? 'Signing Up...' : 'Sign Up'}
            </Button>
          </form>
          <div className="flex items-center gap-4 py-2">
            <Divider className="flex-1" />
            <p className="shrink-0 text-tiny text-default-500">OR</p>
            <Divider className="flex-1" />
          </div>
          <div className="flex flex-col gap-2 mt-4">
            <Link href="#">
              <Button className="w-full" startContent={<Icon icon="flat-color-icons:google" />} variant="bordered">
                Continue with Google
              </Button>
            </Link>
            <Button
              className="w-full"
              startContent={<Icon className="text-default-500" icon="fe:github" width={24} />}
              variant="bordered"
            >
              Continue with Github
            </Button>
          </div>
          <p className="text-center text-small mt-2">
            Already have an account?
            <Link className="ml-2" href="/login" size="sm">
              Log In
            </Link>
          </p>
        </Card>
      </div>
    </div>
  )
}
