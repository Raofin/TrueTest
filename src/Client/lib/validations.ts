import isValidEmail from '@/components/check-valid-email'
import { z } from 'zod'

export const SignInSchema = z.object({
  usernameOrEmail: z
    .string()
    .min(1, 'Email or Username is required')
    .refine(
      value => isValidEmail(value) || /^[\w.-]+$/.test(value),
      'Enter a valid email address or username'
    ),
  password: z
    .string()
    .min(8, 'Password must be at least 8 characters long')
    .max(100, 'Password cannot exceed 100 characters')
    .regex(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$/,
      'Password must contain at least one uppercase letter, lowercase letter, number, and special character'
    )
})

export const signUpSchema = z
  .object({
    username: z
      .string()
      .min(4, 'Username must be at least 4 characters long')
      .max(30, 'Username cannot exceed 30 characters')
      .regex(
        /^\w+$/,
        'Username can only contain letters, numbers, and underscores'
      ),
    email: z
      .string()
      .min(1, 'Email is required')
      .email('Please provide a valid email address'),
    password: z
      .string()
      .min(8, 'Password must be at least 8 characters long')
      .max(100, 'Password cannot exceed 100 characters')
      .regex(
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$/,
        'Password must contain at least one uppercase letter, lowercase letter, number, and special character'
      ),
    confirmPassword: z.string(),
    agreeTerms: z.literal(true, {
      errorMap: () => ({ message: 'You must agree to the terms' })
    })
  })
  .superRefine(({ password, confirmPassword }, ctx) => {
    if (password !== confirmPassword) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: 'Passwords do not match',
        path: ['confirmPassword']
      })
    }
  })