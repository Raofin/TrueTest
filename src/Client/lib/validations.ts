import isValidEmail from '@/components/check-valid-email'
import { z } from 'zod'

export const SignInSchema = z.object({
  usernameOrEmail: z
    .string()
    .refine(
      value => value === '' || isValidEmail(value) || /^[\w.-]+$/.test(value),
      'Enter a valid email address or username'
    ),
  password: z
    .string()
    .refine(val => val === '' || val.length >= 8, {
      message: 'Password must be at least 8 characters long',
    })
    .refine(val => val === '' || val.length <= 100, {
      message: 'Password cannot exceed 100 characters',
    })
    .refine(
      val => val === '' || /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$/.test(val),
      {
        message:
          'Password must contain at least one uppercase letter, lowercase letter, number, and special character',
      }
    )
})

export const signUpSchema = z
  .object({
    username: z
      .string()
      .refine(val => val === '' || val.length >= 4, {
        message: 'Username must be at least 4 characters long',
      })
      .refine(val => val === '' || val.length <= 30, {
        message: 'Username cannot exceed 30 characters',
      })
      .refine(val => val === '' || /^\w+$/.test(val), {
        message: 'Username can only contain letters, numbers, and underscores',
      }),
    email: z
      .string()
      .refine(val => val === '' || /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val), {}),
    password: z
      .string()
      .refine(val => val === '' || val.length >= 8, {
        message: 'Password must be at least 8 characters long',
      })
      .refine(val => val === '' || val.length <= 100, {
        message: 'Password cannot exceed 100 characters',
      })
      .refine(
        val => val === '' || /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$/.test(val),
        {
          message:
            'Password must contain at least one uppercase letter, lowercase letter, number, and special character',
        }
      ),
    confirmPassword: z.string(),
    agreeTerms: z.literal(true, {
      errorMap: () => ({ message: 'You must agree to the terms' }),
    })
  })
  .superRefine(({ password, confirmPassword }, ctx) => {
    if (password && confirmPassword && password !== confirmPassword) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: 'Passwords do not match',
        path: ['confirmPassword'],
      })
    }
  })
