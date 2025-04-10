import isValidEmail from '@/components/check-valid-email'
import { z } from 'zod'


const usernameRegex = /^[\w.-]+$/;

export const SignInSchema = z.object({
  usernameOrEmail: z
    .string()
    .min(4, { message: 'Email or Username is required' })
    .refine(
      (value) => isValidEmail(value) || usernameRegex.test(value),
      { message: 'Enter a valid email address or username.' }
    ),

    password: z
    .string()
    .min(8, { message: 'Password must be at least 8 characters long.' })
    .max(100, { message: 'Password cannot exceed 100 characters.' })
    .regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/, {
      message:
        'Password must contain at least one uppercase letter,  lowercase letter, number, special character.',
    }),
})

export const signUpSchema = z
  .object({
    username: z
      .string()
      .min(4, { message: 'Username must be at least 4 characters long.' })
      .max(30, { message: 'Username cannot exceed 30 characters.' })
      .regex(/^[a-zA-Z0-9_]+$/, {
        message: 'Username can only contain letters, numbers, and underscores.',
      }),
    email: z
      .string()
      .min(1, { message: 'Email is required.' })
      .email({ message: 'Please provide a valid email address.' }),

    password: z
      .string()
      .min(8, { message: 'Password must be at least 8 characters long.' })
      .max(100, { message: 'Password cannot exceed 100 characters.' })
      .regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/, {
        message:
          'Password must contain at least one uppercase , lowercase letter, number and special character.',
      }),
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
