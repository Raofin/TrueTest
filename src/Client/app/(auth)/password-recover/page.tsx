'use client'

import React, { ChangeEvent, useState } from 'react'
import '@/app/globals.css'
import { Button, Card, Input, useDisclosure } from '@heroui/react'
import Link from 'next/link'
import api from '@/utils/api'
import OTPModal from '@/components/ui/Modal/otp-verification'
import { useForm } from 'react-hook-form'
import toast from 'react-hot-toast'
import { AxiosError } from 'axios'
import { useRouter } from 'next/navigation'

const PasswordRecoverPage = () => {
  const { isOpen, onOpen, onOpenChange } = useDisclosure()
  const [error, setError] = useState('')
  const router = useRouter()

  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<{ otp: string }>()
  const [formData, setFormData] = useState<{ email: string }>({
    email: '',
  })
  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }))
    if (name === 'email') {
      setError('')
    }
  }
  const handlePasswordRecovery = async (e: { preventDefault: () => void }) => {
    e.preventDefault()
    try {
      const response = await api.post(`Auth/SendOtp`, { email: formData.email })
      if (response.status === 200) {
        toast.success('OTP sent to your email. Please check your inbox.')
        onOpen()
      } else {
        toast.error('failed to sent otp')
      }
    } catch (err) {
      const error = err as AxiosError
      setError(error.message)
    }
  }
  const onSubmit = async (data: { otp: string }) => {
    try {
      if (!data.otp) {
        toast.error('OTP is required.')
        return
      }
      const verifyResponse = await api.post(`Auth/IsValidOtp`, { email: formData.email, otp: data.otp })
      if (verifyResponse.data.isValidOtp) {
        toast.success('OTP verified successfully!')
        router.push(`/password-recover/reset-password?email=${formData.email}&otp=${data.otp}`)
      } else {
        toast.error(verifyResponse.data?.message || 'Invalid OTP. Please try again.')
      }
    } catch {
      toast.error('failed to verify OTP')
    }
  }
  return (
    <div className="flex items-center justify-center ">
      <Card className="px-8 py-5 shadow-none bg-white dark:bg-[#18181b] " style={{ maxWidth: '370px', flexShrink: 0 }}>
        <form id="#" className={` rounded-lg  w-full `} onSubmit={handlePasswordRecovery}>
          <h1 className="text-2xl font-bold mb-6 text-center">Password Recovery</h1>
          {error && <p className="text-sm text-red-500 mt-1">{error}</p>}
          <Input
            type="email"
            name="email"
            label="Email Address"
            value={formData.email}
            onChange={handleChange}
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
      <OTPModal
        isOpen={isOpen}
        onOpenChange={onOpenChange}
        handleFormSubmit={handleSubmit(onSubmit)}
        control={control}
        errors={errors}
      />
    </div>
  )
}

export default PasswordRecoverPage
