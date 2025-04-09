'use client'

const MAX_EMAIL_LENGTH = 254;

export default function isValidEmail (email: string): boolean {
  if (email.length > MAX_EMAIL_LENGTH) {
    return false;
  }
  return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(email);
};