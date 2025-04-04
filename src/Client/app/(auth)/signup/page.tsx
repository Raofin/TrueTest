'use client'

import React, { useState } from 'react'
import { Button, Input, Checkbox, Link, Divider, Card, Form, useDisclosure } from '@heroui/react'
import { Icon } from '@iconify/react'
import '@/app/globals.css'
import toast, { Toaster } from 'react-hot-toast'
import api from '@/app/utils/api'
import axios from 'axios'
import { useRouter } from 'next/navigation'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import OTPModal from '@/app/components/ui/Modal/otp-verification'
import { setAuthToken } from '@/app/utils/auth'

const signUpSchema = z
  .object({
    username: z
      .string()
      .min(4, 'Username must be at least 4 characters')
      .max(20, 'Username must be at most 20 characters')
      .regex(/^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores'),

    email: z
      .string()
      .email('Invalid email address')
      .refine((email) => email.endsWith('.com'), { message: 'Email must end with .com' }),

    password: z
      .string()
      .min(8, 'Password must be at least 8 characters')
      .regex(/[a-z]/, 'Password must contain at least one lowercase letter')
      .regex(/[A-Z]/, 'Password must contain at least one uppercase letter')
      .regex(/\d/, 'Password must contain at least one number')
      .regex(/[!@#$%^&*(),.?":{}|<>]/, 'Password must contain at least one special character'),

    confirmPassword: z.string(),

    agreeTerms: z.literal(true, {
      errorMap: () => ({ message: 'You must agree to the terms' }),
    }),
  })
  .superRefine(({ password, confirmPassword }, ctx) => {
    if (password !== confirmPassword) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: 'Passwords do not match',
        path: ['confirmPassword'],
      })
    }
  })

type FormData = z.infer<typeof signUpSchema>

export default function Signup() {
  const [isVisible, setIsVisible] = useState(false)
  const [isConfirmVisible, setIsConfirmVisible] = useState(false)
  const [loading, setLoading] = useState(false)
  const { isOpen, onOpen, onOpenChange } = useDisclosure()
  const router = useRouter()
  const [uniqueemailerror,setUniqueEmailError]=useState("");
  const [uniqueusernameerror,setUniqueUsernameError]=useState("");

  const {
    register,
    handleSubmit,
    formState: { errors },
    setError,
    trigger,
    watch,
  } = useForm<FormData>({
    resolver: zodResolver(signUpSchema),
    mode: 'onBlur',
  })

  const {
    handleSubmit: handleOtpSubmit,
    control,
    formState: { errors: otpErrors },
  } = useForm<{ otp: string }>()

  const checkUserUniqueness = async (field: 'username' | 'email', value: string) => {
    if (!value) return

    try {
      const response=await api.post('/Auth/IsUserUnique', { [field]: value })
       if(response.data.isUnique===false){
        if(field==='username')
        setUniqueUsernameError("Username is already taken")
      else setUniqueEmailError("Email is already taken")
       }else{
          if (field === 'username') setUniqueUsernameError("")
          else setUniqueEmailError("")

       }
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.data?.errors) {
        Object.values(error.response.data.errors).forEach((errorMessages) => {
          if (Array.isArray(errorMessages)) {
            errorMessages.forEach((message) => {
              if (message.includes(field === 'username' ? 'Username' : 'Email')) {
                setError(field, { type: 'manual', message })
              }
            })
          }
        })
      }
      return false
    }
  }

  const handleFieldBlur = async (field: 'username' | 'email') => {
    const value = watch(field)
    if (field === 'username') setUniqueUsernameError("")
      if (field === 'email') setUniqueEmailError("")
    
    if (value) {
      await trigger(field)
      if (!errors[field]) {
        await checkUserUniqueness(field, value)
      }
    }
  }

  const onSubmit = async (data: FormData) => {
    setLoading(true)
    try {
      const response = await api.post('/Auth/SendOtp', { email: data.email })
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

  const onOtpSubmit = async (data: { otp: string }) => {
    try {
      if (!data.otp) {
        toast.error('OTP is required')
        return
      }

      const formData = watch()
      const verifyResponse = await api.post('/Auth/IsValidOtp', {
        email: formData.email,
        otp: data.otp,
      })

      if (verifyResponse.data.isValidOtp) {
        toast.success('OTP verified successfully!')
       const response= await api.post('/Auth/Register', {
          username: formData.username,
          email: formData.email,
          password: formData.password,
          otp: data.otp,
        })
        if(response.status===200){
        toast.success('Signup successful!')
        router.push('/userprofile/setup')
        setAuthToken(response.data.token)}
        else router.push('/signup')
      } else {
        toast.error(verifyResponse.data?.message || 'Invalid OTP. Please try again.')
      }
    } catch {
      toast.error('Failed to Register.Please try again.')
    }
  }

  return (
    <div>
      <div className="container mx-auto px-4 py-12">
        <Card className="max-w-sm mx-auto rounded-xl px-8 py-5 shadow-none bg-white dark:bg-[#18181b]">
          <h2 className="text-2xl font-bold text-center my-4">Sign Up</h2>
          <Form className="flex flex-col gap-4" onSubmit={handleSubmit(onSubmit)}>
            <Input
              {...register('username')}
              onBlur={() => handleFieldBlur('username')}
              isRequired
              label="Username"
              type="text"
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              />
            {errors.username && <p className="text-sm text-red-500 mt-1">{errors.username.message}</p>}
            {uniqueusernameerror && <p className='text-red-500'>{uniqueusernameerror}</p>}
            <Input
              {...register('email')}
              onBlur={() => handleFieldBlur('email')}
              isRequired
              label="Email"
              type="email"
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              />
            {errors.email && <p className="text-sm text-red-500 mt-1">{errors.email.message}</p>}
              {uniqueemailerror && <p className='text-red-500'>{uniqueemailerror}</p>}

            <Input
              {...register('password')}
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
              label="Password"
              type={isVisible ? 'text' : 'password'}
            />
            {errors.password && <p className="text-sm text-red-500">{errors.password.message}</p>}

            <Input
              {...register('confirmPassword')}
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
              type={isConfirmVisible ? 'text' : 'password'}
            />
            {errors.confirmPassword && <p className="text-sm text-red-500">{errors.confirmPassword.message}</p>}

            <Checkbox {...register('agreeTerms')} className="py-4" size="sm">
              I agree with the &nbsp;
              <Link href="#" size="sm">
                Terms
              </Link>
              &nbsp; and &nbsp;
              <Link href="#" size="sm">
                Privacy Policy
              </Link>
            </Checkbox>
            {errors.agreeTerms && <p className="text-sm text-red-500">{errors.agreeTerms.message}</p>}

            <Button className="w-full" color="primary" type="submit" isDisabled={loading}>
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
        handleFormSubmit={handleOtpSubmit(onOtpSubmit)}
        control={control}
        errors={otpErrors}
      />
      <Toaster />
    </div>
  )
}
