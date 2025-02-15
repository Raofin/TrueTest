'use client'

import React from 'react'
import { Button, Input, Checkbox, Link, Form, Divider } from '@heroui/react'
import { Icon } from '@iconify/react'
import '../../styles/globals.css'
import {useRouter} from "next/navigation";
export default function Component() {
  const [isVisible, setIsVisible] = React.useState(false)
  const toggleVisibility = () => setIsVisible(!isVisible)
  const [user,setUser]=React.useState({
    email:"",password:"",
  })
  const router =useRouter();
  const handleSubmit = async(event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    router.push('/admin_dashboard')
  }

  return (
    <div className="flex h-full w-full items-center justify-center">
      <div className="flex w-full max-w-sm flex-col gap-4 rounded-large bg-content1 px-8 pb-10 pt-6 shadow-small">
        <div className="flex flex-col gap-1">
          <h1 className="text-large font-medium">Sign in to your account</h1>
        </div>
        <Form className="flex w-full flex-wrap md:flex-nowrap gap-4 flex-col" validationBehavior="native" onSubmit={handleSubmit}>
          <Input isRequired label="Email" name="email" type="email" variant="bordered" />
          <Input isRequired endContent={
              <button type="button" onClick={toggleVisibility}>
                {isVisible ? (
                  <Icon className="pointer-events-none text-2xl text-default-400" icon="solar:eye-closed-linear" />
                ) : (
                  <Icon className="pointer-events-none text-2xl text-default-400" icon="solar:eye-bold" />
                )}
              </button>
            }
            label="Password" name="password" type={isVisible ? 'text' : 'password'} variant="bordered"/>
          <div className="flex w-full items-center justify-between px-1 py-2">
            <Checkbox name="remember" size="sm">
              Remember me
            </Checkbox>
            <Link className="text-default-500" href="#" size="sm">
              Forgot password?
            </Link>
          </div>
          <Button className="w-full" color="primary" type="submit">
            Sign In
          </Button>
        </Form>
        <div className="flex items-center gap-4 py-2">
          <Divider className="flex-1" />
          <p className="shrink-0 text-tiny text-default-500">OR</p>
          <Divider className="flex-1" />
        </div>
        <div className="flex flex-col gap-2">
          <Link href='#'><Button className='w-full' startContent={<Icon icon="flat-color-icons:google" />} variant="bordered">
            Continue with Google
          </Button></Link>
          <Button startContent={<Icon className="text-default-500" icon="fe:github" width={24} />} variant="bordered">
            Continue with Github
          </Button>
        </div>
        <p className="text-center text-small">
          Need to create an account?&nbsp;
          <Link href="/registration" size="sm">
            Sign Up
          </Link>
        </p>
      </div>
    </div>
  )
}
