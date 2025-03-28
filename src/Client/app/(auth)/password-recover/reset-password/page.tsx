'use client'


import isValidPassword from '@/app/components/check-valid-password'
import api from '@/app/components/utils/api'
import { Button, Input, Link } from '@heroui/react'
import { Icon } from '@iconify/react/dist/iconify.js'
import { AxiosError } from 'axios'
import { useRouter, useSearchParams } from 'next/navigation'
import { FormEvent, useState } from 'react'
import toast from 'react-hot-toast'


export default function Component() {
  const searchParams = useSearchParams()
  const email = searchParams.get('email')
  const otp = searchParams.get('otp')
  const [error, setError] = useState('')
  const [newPassword, setNewPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const router = useRouter()
  const [isVisible, setIsVisible] = useState<boolean>(false)
  const [isConfirmVisible, setIsConfirmVisible] = useState<boolean>(false)


  const handleResetPassword = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault()
    if (newPassword !== confirmPassword) {
      setError('Passwords do not match.')
      return
    }
    if (!isValidPassword(newPassword)) {
      setError(
        'Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.'
      )
      return
    }
    try {
      const response = await api.post('Auth/PasswordRecovery', {
        email: email,
        newPassword: newPassword,
        otp: otp,
      })
      if (response.status === 200) {
        toast.success('password recover successfully')
        router.push('/login')
      } else {
        toast.error('failed to recover password.Please try again later!')
      }
    } catch (err) {
      const error = err as AxiosError
      setError(error.message)
    }
  }
  return (
    <div className="flex h-screnn w-full items-center justify-center mb-16">
      <div className="flex w-full max-w-sm flex-col mt-16 gap-4 rounded-large px-8 py-5 bg-white dark:bg-[#18181b] ">
        <div className="flex flex-col gap-3">
          <h1 className="text-2xl font-bold text-center mb-6">Reset Password</h1>
        </div>
        <form id="#" className={`flex w-full flex-wrap md:flex-nowrap gap-4 flex-col`} onSubmit={handleResetPassword}>
          {error && <p className="text-sm text-red-500 mt-1">{error}</p>}
          <Input
            isRequired
            label="Email Address"
            name="email"
            type="email"
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            defaultValue={email || ''}
          />
          <Input
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            isRequired
            endContent={
              <button type="button" onClick={() => setIsVisible(!isVisible)}>
                <Icon
                  className="text-2xl text-default-400"
                  icon={isVisible ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                />
              </button>
            }
            label="New Password"
            name="newpassword"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
            type={isVisible ? 'text' : 'password'}
          />
          <Input
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            isRequired
            endContent={
              <button type="button" onClick={() => setIsConfirmVisible(!isConfirmVisible)}>
                <Icon
                  className="text-2xl text-default-400"
                  icon={isConfirmVisible ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                />
              </button>
            }
            label="Confirm Password"
            name="newconfirmpassword"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            type={isConfirmVisible ? 'text' : 'password'}
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
