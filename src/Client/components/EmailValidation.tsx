'use client'

const MAX_EMAIL_LENGTH = 254;

export default function isValidEmail(email: string): boolean {
  if (email.length > MAX_EMAIL_LENGTH) {
    return false;
  }

  const safeEmailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;
  return safeEmailRegex.test(email);
}