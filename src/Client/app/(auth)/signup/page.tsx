'use client'

import React from 'react'

import AuthForm from '@/components/forms/AuthForm'
import { signUpSchema } from '@/lib/validations'

const SignUp = () => {
  return (
    <div className="flex h-full w-full items-center justify-center">
      <div className={`mt-12 flex w-full max-w-sm flex-col gap-4 rounded-large px-8 py-5 bg-white dark:bg-[#18181b]`}>
        <div className="flex flex-col gap-1">
          <h1 className="text-2xl font-bold my-3 text-center ">Sign Up</h1>
        </div>
        <AuthForm formType="SIGN_UP" schema={signUpSchema} />
      </div>
    </div>
  )
}

export default SignUp
