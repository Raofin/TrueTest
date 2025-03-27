'use client'

import React, { useState } from 'react'
import { Button, Input, Checkbox, Link, Form, Divider } from '@heroui/react'
import { Icon } from '@iconify/react'
import { useAuth } from '@/app/context/AuthProvider'

export default function LoginComponent() {
  const [isVisible, setIsVisible] = useState(false)
  const [user, setUser] = useState({ email: '', password: '' })
  const toggleVisibility = () => setIsVisible((prev) => !prev)
  const [error, setError] = useState('')
  const { login } = useAuth();
  const handleLogin =  (e: React.FormEvent) => {
    e.preventDefault()
    if (!user.email || !user.password) {
      setError('Email and password are required.');
      return;
    }
    login(user.email, user.password, setError);
  }

  return (
    <div className="flex h-full w-full items-center justify-center">
      <div className={`mt-12 flex w-full max-w-sm flex-col gap-4 rounded-large px-8 py-5 bg-white dark:bg-[#18181b]`}>
        <div className="flex flex-col gap-1">
          <h1 className="text-2xl font-bold my-3 text-center ">Log In</h1>
        </div>
        <Form
          onSubmit={handleLogin}
          id="#"
          className="flex w-full flex-wrap gap-4 flex-col"
          validationBehavior="native"
        >
          {error && <p className="text-red-500">{error}</p>}
          <Input
            isRequired
            label="Username or Email Address"
            name="email"
            type="email"
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            value={user.email}
            onChange={(e: { target: { value: string } }) => setUser((prev) => ({ ...prev, email: e.target.value }))}
          />
          <Input
            isRequired
            label="Password"
            name="password"
            type={isVisible ? 'text' : 'password'}
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            value={user.password}
            onChange={(e: { target: { value: string } }) => setUser((prev) => ({ ...prev, password: e.target.value }))}
            endContent={
              <button type="button" onClick={toggleVisibility}>
                <Icon
                  className="pointer-events-none text-2xl text-default-400"
                  icon={isVisible ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                />
              </button>
            }
          />
          <div className="flex w-full items-center justify-between px-1 py-2">
            <Checkbox name="remember" size="sm">
              <p>Remember me</p>
            </Checkbox>
            <Link className="text-default-500" href="/password-recover" size="sm">
              Forgot password?
            </Link>
          </div>
          <Button className="w-full" color="primary" type="submit">
            Log In
          </Button>
        </Form>
        <div className="flex items-center gap-4 py-2">
          <Divider className="flex-1" />
          <p className="shrink-0 text-tiny text-default-500">OR</p>
          <Divider className="flex-1" />
        </div>
        <div className="flex flex-col gap-2">
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
        <p className="text-center text-small">
          Need to create an account?&nbsp;
          <Link href="/signup" size="sm">
            Sign Up
          </Link>
        </p>
      </div>
    </div>
  )
}
