import React, { Dispatch, useCallback } from 'react'
import { Input, Checkbox, Link } from '@heroui/react'
import { Icon } from '@iconify/react'
import { FieldValues, Path, UseFormRegister } from 'react-hook-form'
import ROUTES from '@/constants/route'

interface SignInFormFieldsProps<T extends FieldValues> {
  register: UseFormRegister<T>
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  errors: any
  isVisible: boolean
  setIsVisible: Dispatch<React.SetStateAction<boolean>>
  rememberMe: boolean
  setRememberMe: Dispatch<React.SetStateAction<boolean>>
}

const SignInFormFields = <T extends FieldValues>({
  register,
  errors,
  isVisible,
  setIsVisible,
  rememberMe,
  setRememberMe,
}: SignInFormFieldsProps<T>) => {
  const renderSignInFields = useCallback(() => {
    return (
      <>
        <Input
          {...register('usernameOrEmail' as Path<T>)}
          isRequired
          label="Username or Email Address"
          type="text"
          className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
        />
        {errors.usernameOrEmail && (
          <p className="text-sm text-red-500 mt-1">{errors.usernameOrEmail.message as string}</p>
        )}
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
        {errors.password && <p className="text-sm text-red-500">{errors.password.message as string}</p>}
        <div className="flex w-full items-center justify-between px-1 py-2">
          <Checkbox name="remember" size="sm" isSelected={rememberMe} onChange={(e) => setRememberMe(e.target.checked)}>
            <p>Remember me</p>
          </Checkbox>
          <Link className="text-default-500" href={ROUTES.FORGOT_PASSWORD} size="sm">
            Forgot password?
          </Link>
        </div>
      </>
    )
  }, [register, errors.usernameOrEmail, errors.password, isVisible, rememberMe, setIsVisible, setRememberMe])

  return renderSignInFields()
}

export default SignInFormFields
