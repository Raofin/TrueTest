'use client'

export default function isValidEmail (email: string): boolean {
  return /^\w+([\.-]?\w+)@\w+([\.-]?\w+)(\.\w{2,3})+$/.test(email);
};