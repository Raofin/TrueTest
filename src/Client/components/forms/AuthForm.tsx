'use client'

import { getAuthToken } from '@/utils/auth'
import ROUTES from '@/constants/route'
import { useAuth } from '@/context/AuthProvider'
import { Button, Checkbox, Divider, Input, Link, useDisclosure } from '@heroui/react'
import { zodResolver } from '@hookform/resolvers/zod'
import { useRouter } from 'next/navigation'
import React, { useState } from 'react'
import { FieldValues, Path, useForm } from 'react-hook-form'
import { ZodType } from 'zod'
import api from '@/utils/api'
import { Icon } from '@iconify/react'
import toast from 'react-hot-toast'
import axios from 'axios'
import OTPModal from '@/components/ui/Modal/otp-verification'
import { setAuthToken } from '@/utils/auth'

interface AuthFormProps<T extends FieldValues> {
  schema: ZodType<T>
  formType: 'SIGN_IN' | 'SIGN_UP'
}

const AuthForm = <T extends FieldValues>({ schema, formType }: AuthFormProps<T>) => {
  const buttonText = formType === 'SIGN_IN' ? 'Sign In' : 'Sign Up'
  const toggleVisibility = () => setIsVisible((prev) => !prev)
  const { login, user: authenticatedUser } = useAuth()
  const router = useRouter()
  const [error, setError] = useState('')
  const [formData, setFormData] = useState<T | null>(null)
  const [isVisible, setIsVisible] = useState(false)
  const [isConfirmVisible, setIsConfirmVisible] = useState(false)
  const [loading, setLoading] = useState(false)
  const { isOpen, onOpen, onOpenChange } = useDisclosure()
  const [uniqueemailerror, setUniqueEmailError] = useState('')
  const [uniqueusernameerror, setUniqueUsernameError] = useState('')
  const [otpVerified, setOtpVerified] = useState(false)
  const [rememberMe, setRememberMe] = useState(false)
  const {
    register,
    handleSubmit,
    formState: { errors },
    trigger,
    setError: setUniqueError,
    watch,
  } = useForm<T>({
    resolver: zodResolver(schema),
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
      const response = await api.post(ROUTES.ISUSERUNIQUE, { [field]: value })
      if (response.data.isUnique === false) {
        if (field === 'username') setUniqueUsernameError('Username is already taken')
        else setUniqueEmailError('Email is already taken')
      } else {
        if (field === 'username') setUniqueUsernameError('')
        else setUniqueEmailError('')
      }
    } catch (error) {
      if (axios.isAxiosError(error) && error.response?.data?.errors) {
        Object.values(error.response.data.errors).forEach((errorMessages) => {
          if (Array.isArray(errorMessages)) {
            errorMessages.forEach((message) => {
              if (message.includes(field === 'username' ? 'Username' : 'Email')) {
                setUniqueError(field as Path<T>, { type: 'manual', message })
              }
            })
          }
        })
      }
      return false
    }
  }

  const handleFieldBlur = async (field: 'username' | 'email') => {
    const value = watch(field as Path<T>)
    if (field === 'username') setUniqueUsernameError('')
    if (field === 'email') setUniqueEmailError('')

    if (value) {
      await trigger(field as Path<T>)
      if (!errors[field]) {
        await checkUserUniqueness(field, value)
      }
    }
  }

  const onSubmit = async (data: T) => {
    setLoading(true)
    try {
      const response = await api.post(ROUTES.SENDOTP, { email: data.email })
      if (response.status === 200) {
        console.log('first time SendOtp')
        toast.success('OTP sent to your email. Please check your inbox.')
        setFormData(data)
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
    if (otpVerified) return
    setOtpVerified(true)
    try {
      if (!data.otp) {
        toast.error('OTP is required')
        return
      }
      const verifyResponse = await api.post(ROUTES.ISVALIDOTP, {
        email: formData?.email,
        otp: data.otp,
      })

      if (verifyResponse.data.isValidOtp) {
        console.log('first time isValidOtp')
        setOtpVerified(true)
        setFormData(null)
        toast.success('OTP verified successfully!')
        const response = await api.post(ROUTES.REGISTER, {
          username: formData?.username,
          email: formData?.email,
          password: formData?.password,
          otp: data.otp,
        })
        if (response.status === 200) {
          toast.success('Signup successful!')
          router.push(ROUTES.PROFILE_SETUP)
          setAuthToken(response.data.token,false)
        } else router.push(ROUTES.SIGN_UP)
      } else {
        toast.error(verifyResponse.data?.message || 'Invalid OTP. Please try again.')
      }
    } catch {
      toast.error('Failed to Register.Please try again.')
    }
  }

  const handleSignin = async (data: T) => {
    if (!data.email || !data.password) {
      setError('Email and password are required.')
      return
    }
    if (getAuthToken()) {
      if (authenticatedUser?.roles.includes('Admin')) {
        router.push(ROUTES.OVERVIEW)
      } else {
        router.push(ROUTES.HOME)
      }
    }
    login(data.email, data.password, setError,rememberMe)
  }
  return (
    <div>
      <form
        onSubmit={formType === 'SIGN_IN' ? handleSubmit(handleSignin) : handleSubmit(onSubmit)}
        className="flex w-full flex-wrap gap-4 flex-col"
      >
        {error && <p className="text-red-500">{error}</p>}
        {formType === 'SIGN_IN' ? (
          <>
            <Input
              {...register('email' as Path<T>)}
              isRequired
              label="Username or Email Address"
              type="email"
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            />
            <Input
              isRequired
              label="Password"
              type={isVisible ? 'text' : 'password'}
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              {...register('password' as Path<T>)}
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
              <Checkbox name="remember" size="sm" isSelected={rememberMe} onChange={(e)=>setRememberMe(e.target.checked)}>
                <p>Remember me</p>
              </Checkbox>
              <Link className="text-default-500" href="/password-recover" size="sm">
                Forgot password?
              </Link>
            </div>
          </>
        ) : (
          <>
            <Input
              {...register('username' as Path<T>)}
              onBlur={() => handleFieldBlur('username')}
              isRequired
              label="Username"
              type="text"
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            />
            {errors.username && <p className="text-sm text-red-500 mt-1">{errors.username.message as Path<T>}</p>}
            {uniqueusernameerror && <p className="text-red-500">{uniqueusernameerror}</p>}
            <Input
              {...register('email' as Path<T>)}
              onBlur={() => handleFieldBlur('email')}
              isRequired
              label="Email"
              type="email"
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            />
            {errors.email && <p className="text-sm text-red-500 mt-1">{errors.email.message as Path<T>}</p>}
            {uniqueemailerror && <p className="text-red-500">{uniqueemailerror}</p>}

            <Input
              {...register('password' as Path<T>)}
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
            {errors.password && <p className="text-sm text-red-500">{errors.password.message as Path<T>}</p>}

            <Input
              {...register('confirmPassword' as Path<T>)}
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
            {errors.confirmPassword && (
              <p className="text-sm text-red-500">{errors.confirmPassword.message as Path<T>}</p>
            )}

            <Checkbox {...register('agreeTerms' as Path<T>)} className="py-4" size="sm">
              I agree with the &nbsp;
              <Link href="#" size="sm">
                Terms
              </Link>
              &nbsp; and &nbsp;
              <Link href="#" size="sm">
                Privacy Policy
              </Link>
            </Checkbox>
            {errors.agreeTerms && <p className="text-sm text-red-500">{errors.agreeTerms.message as Path<T>}</p>}
          </>
        )}

        <Button className="w-full text-white" color="primary" type="submit" isDisabled={loading}>
          {!loading ? buttonText : 'Processing...'}
        </Button>
        <div className="flex items-center gap-4 py-2">
          <Divider className="flex-1" />
          <p className="shrink-0 text-tiny text-default-500">OR</p>
          <Divider className="flex-1" />
        </div>
        <div className="flex flex-col gap-2">
          <Button className="w-full" startContent={<Icon icon="flat-color-icons:google" />} variant="bordered">
            Continue with Google
          </Button>

          <Button
            className="w-full"
            startContent={<Icon className="text-default-500" icon="fe:github" width={24} />}
            variant="bordered"
          >
            Continue with Github
          </Button>
        </div>
        {formType === 'SIGN_IN' ? (
          <p className="w-full flex gap-2 text-center text-sm items-center justify-center">
            Need to create an account?
            <Link href={ROUTES.SIGN_UP}>Sign up</Link>
          </p>
        ) : (
          <p className="w-full flex gap-2 text-center text-sm items-center justify-center">
            Already have an account?
            <Link href={ROUTES.SIGN_IN}>Sign in</Link>
          </p>
        )}
      </form>
      <OTPModal
        isOpen={isOpen}
        onOpenChange={onOpenChange}
        handleFormSubmit={handleOtpSubmit(onOtpSubmit)}
        control={control}
        errors={otpErrors}
      />
    </div>
  )
}

export default AuthForm
