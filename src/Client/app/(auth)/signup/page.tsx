'use client'

import React, { ChangeEvent, FormEvent, useEffect, useState } from 'react'
import { Input, Checkbox, Link, Card, Form, useDisclosure, Button, Divider } from '@heroui/react'
import { Icon } from '@iconify/react'
import '@/app/globals.css'
import toast, { Toaster } from 'react-hot-toast'
import api from '@/app/utils/api'
import axios from 'axios'
import { useRouter } from 'next/navigation'
import { useForm } from 'react-hook-form'
import isValidPassword from '@/app/components/check-valid-password'
import OTPModal from '@/app/components/ui/Modal/otp-verification'

interface FormData {
  username: string
  email: string
  password: string
  confirmPassword: string
  agreeTerms: boolean
}

export default function Signup() {
  const [isVisible, setIsVisible] = useState<boolean>(false)
  const [isConfirmVisible, setIsConfirmVisible] = useState<boolean>(false)
  const [loading, setLoading] = useState<boolean>(false)
  const [message, setMessage] = useState<string>('')
  const { isOpen, onOpen, onOpenChange } = useDisclosure()
  const [emailError, setEmailError] = useState<string>('')
  const [userError, setUserError] = useState<string>('')
  const [passwordError, setPasswordError] = useState<string>('')

  const router = useRouter()
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<{ otp: string }>()
  const [formData, setFormData] = useState<FormData>({
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
    agreeTerms: false,
  })
  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target
    setFormData((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }))

    if (name === 'email') {
      setEmailError('')
    } else if (name === 'username') {
      setUserError('')
    } else if (name === 'password' || name === 'confirmPassword') {
      setPasswordError('')
      setMessage('')
    }
  }
  const checkUserOrEmail = async (field: 'username' | 'email', value: string) => {
    if (!value) return
    try {
      await api.post(`/Auth/IsUserUnique`, { [field]: value })
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.data?.errors) {
        Object.values(error.response?.data?.errors).forEach((errorMessages) => {
          if (Array.isArray(errorMessages)) {
            errorMessages.forEach((message) => {
              if (message.includes(field === 'username' ? 'Username' : 'Email')) {
                // eslint-disable-next-line @typescript-eslint/no-unused-expressions
                field === 'username' ? setUserError(message) : setEmailError(message)
              }
            })
          }
        })
      }
    }
  }

  useEffect(() => {
    const timeoutId = setTimeout(() => checkUserOrEmail('username', formData.username), 1000)
    return () => clearTimeout(timeoutId)
  }, [formData.username])

  useEffect(() => {
    const timeoutId = setTimeout(() => checkUserOrEmail('email', formData.email), 1000)
    return () => clearTimeout(timeoutId)
  }, [formData.email])

  const handleSignUp = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault()
    setLoading(true)
    setMessage('')
    if (!formData.agreeTerms) {
      setMessage('You must agree to the terms.')
      setLoading(false)
      return
    }
    if (formData.password !== formData.confirmPassword) {
      setPasswordError('Passwords do not match.')
      setLoading(false)
      return
    }
    if (!isValidPassword(formData.password)) {
      setPasswordError(
        'Password must be at least 8 characters long and contain at least one lowercase letter, one uppercase letter, one digit, and one special character.'
      )
      setLoading(false)
      return
    }
    try {
      const response = await api.post(`Auth/SendOtp`, { email: formData.email })
      if (response.status === 200) {
        toast.success('OTP sent to your email. Please check your inbox.')
        onOpen()
      }
    } catch (error) {
      if (axios.isAxiosError(error)) {
        toast.error(error.response?.data?.message || 'Failed to send OTP.')
      }
    } finally {
      setLoading(false)
    }
  }
  const onSubmit = async (data: { otp: string }) => {
    try {
      if (!data.otp) {
        toast.error('Email and OTP are required.')
        return
      }
      const verifyResponse = await api.post(`Auth/IsValidOtp`, { email: formData.email, otp: data.otp })
      if (verifyResponse.data.isValidOtp) {
        toast.success('OTP verified successfully!')
        await api.post(`Auth/Register`, {
          username: formData.username,
          email: formData.email,
          password: formData.password,
          otp: data.otp,
        })
        toast.success('Signup successful!')
        router.push('/userprofile/setup')
      } else {
        toast.error(verifyResponse.data?.message || 'Invalid OTP. Please try again.')
        router.push('/signup')
      }
    } catch {
      toast.error('failed to verify OTP')
    }
  }
  return (
    <div>
      <div className="container mx-auto px-4 py-12 ">
        <Card className={`max-w-sm mx-auto  rounded-xl px-8 py-5 shadow-none bg-white dark:bg-[#18181b]`}>
          <h2 className="text-2xl font-bold text-center my-4 ">Sign Up</h2>
          <Form id="#" className="flex flex-col gap-4" onSubmit={handleSignUp}>
            {message && <p className="text-sm text-red-500 mt-1">{message}</p>}
            <Input
              isRequired
              label="Username"
              name="username"
              type="text"
              value={formData.username}
              onChange={handleChange}
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            />
            {userError && <p className="text-sm text-red-500 mt-1">{userError}</p>}
            <Input
              isRequired
              label="Email"
              name="email"
              type="email"
              value={formData.email}
              onChange={handleChange}
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            />
            {emailError && <p className="text-sm text-red-500 mt-1">{emailError}</p>}
            <Input
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              isRequired
              endContent={
                <button type="button" onClick={()=>setIsVisible(!isVisible)}>
                  <Icon
                    className="text-2xl text-default-400"
                    icon={isVisible ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                  />
                </button>
              }
              label="Password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              type={isVisible ? 'text' : 'password'}
            />
            {passwordError && <p className="text-center text-sm text-red-500">{passwordError}</p>}

            <Input
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              label="Confirm Password"
              name="confirmPassword"
              value={formData.confirmPassword}
              endContent={
                <button type="button" onClick={()=>setIsConfirmVisible(!isConfirmVisible)}>
                  <Icon
                    className="text-2xl text-default-400"
                    icon={isConfirmVisible ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                  />
                </button>
              }
              onChange={handleChange}
              type={isConfirmVisible ? 'text' : 'password'}
            />
           <Checkbox isRequired className="py-4" size="sm" name="agreeTerms"  isSelected={formData.agreeTerms}
  onValueChange={(isChecked) => setFormData(prev => ({...prev, agreeTerms: isChecked}))}>
              I agree with the &nbsp;
              <Link href="#" size="sm">
                Terms
              </Link>
              &nbsp; and &nbsp;
              <Link href="#" size="sm">
                Privacy Policy
              </Link>
            </Checkbox>
            <Button className='w-full' color="primary" type="submit" isDisabled={loading}>
              {loading ? 'Signing Up...' : 'Sign Up'}
            </Button>
          </Form>
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
      <OTPModal
        isOpen={isOpen}
        onOpenChange={onOpenChange}
        handleFormSubmit={handleSubmit(onSubmit)}
        control={control}
        errors={errors}
      />
      <Toaster />
    </div>
  )
}
