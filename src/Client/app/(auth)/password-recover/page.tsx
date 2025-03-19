'use client'

import React, { useState } from 'react'
import '@/app/globals.css'
import { Button, Card } from '@heroui/react'
import Link from 'next/link'


const PasswordRecoverPage = () => {
  const [contactInfo, setContactInfo] = useState('')

  return (
    <div className="flex items-center justify-center h-[600px]">
      <Card>
      <form id="#" className={`p-8 rounded-lg shadow-lg max-w-md w-full `}>
        <h1 className="text-2xl font-semibold mb-6 text-center">Password Recovery</h1>
        <input
          type="email"
          placeholder="Enter your email"
          value={contactInfo}
          onChange={(e) => setContactInfo(e.target.value)}
          required
          className="w-full p-3 border border-gray-300 rounded mb-4"
        />
        <Button className="my-4 w-full" color="primary" type="submit">
          <Link href="/password-recover/reset-password">Verify Email</Link>
        </Button>
        <p>
          Want to create a new account?{' '}
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
