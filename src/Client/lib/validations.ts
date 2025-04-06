import { z } from 'zod'

export const SignInSchema = z.object({
  email: z
    .string()
    .min(1, { message: 'Email is required' })
    .regex(/^\w+([\.-]?\w+)@\w+([\.-]?\w+)(\.\w{2,3})+$/,{ message: 'Please provide a valid email address.' }),

    password: z
    .string()
    .min(8, { message: 'Password must be at least 8 characters long.' })
    .max(100, { message: 'Password cannot exceed 100 characters.' })
    .regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/, {
      message: 'Password must contain at least one uppercase letter,one lowercase letter,one number, one special character.',
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
      .regex(/^\w+([\.-]?\w+)@\w+([\.-]?\w+)(\.\w{2,3})+$/,{ message: 'Please provide a valid email address.' }),

    password: z
      .string()
      .min(8, { message: 'Password must be at least 8 characters long.' })
      .max(100, { message: 'Password cannot exceed 100 characters.' })
      .regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/, {
        message: 'Password must contain at least one uppercase letter,one lowercase letter,one number, one special character.',
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
