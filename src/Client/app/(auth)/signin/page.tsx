'use client'

import React from 'react'

import AuthForm from '@/components/forms/AuthForm'
import { SignInSchema } from '@/lib/validations'

const SignIn = () => {
  return (
    <div className="flex h-full w-full items-center justify-center">
      <div className={`mt-12 flex w-full max-w-sm flex-col gap-4 rounded-large px-8 py-5 bg-white dark:bg-[#18181b]`}>
        <div className="flex flex-col gap-1">
          <h1 className="text-2xl font-bold my-3 text-center ">Log In</h1>
        </div>
        <AuthForm formType="SIGN_IN" schema={SignInSchema} />
      </div>
    </div>
  )
}

export default SignIn
