'use client'

import React from 'react'
import '@/app/globals.css'
import { Button, Card, Input, Link } from '@heroui/react'

const PasswordRecoverPage = () => {
  return (
    <div className="flex items-center justify-center ">
      <Card className="px-8 py-5 shadow-none bg-white dark:bg-[#18181b] " style={{ maxWidth: '370px', flexShrink: 0 }}>
        <form id="#" className={` rounded-lg  w-full `}>
          <h1 className="text-2xl font-bold mb-6 text-center">Password Recovery</h1>

          <Input
            type="email"
            name="email"
            label="Email Address"
            required
            className="w-full mb-4 bg-[#eeeef0] dark:bg-[#24242b] rounded-xl"
          />
          <Button className="my-4 w-full" color="primary" type="submit">
            Verify Email
          </Button>
          <p>
            Want to create a new account?
            <Link className="text-blue-500 ml-2" href="/signup">
              Sign Up
            </Link>
          </p>
        </form>
      </Card>
    </div>
  )
}

export default PasswordRecoverPage
