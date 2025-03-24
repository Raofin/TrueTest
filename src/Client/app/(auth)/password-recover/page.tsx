'use client'

import React, { useState } from 'react'
import '@/app/globals.css'
import { Button, Card, Input } from '@heroui/react'
import Link from 'next/link'

const PasswordRecoverPage = () => {
  const [contactInfo, setContactInfo] = useState('')

  return (
    <div className="flex items-center justify-center ">
      <Card className="px-8 py-5">
        <form id="#" className={` rounded-lg  w-full `}>
          <h1 className="text-2xl font-bold mb-6 text-center">Password Recovery</h1>
          <Input
            type="email"
            label="Email Address"
            value={contactInfo}
            onChange={(e) => setContactInfo(e.target.value)}
            required
            className="w-full mb-4 bg-[#eeeef0] dark:bg-[#24242b] rounded-xl"
          />
          <Button className="my-4 w-full" color="primary" type="submit">
            <Link href="/password-recover/reset-password">Verify Email</Link>
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
