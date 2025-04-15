'use client'

import api from '@/lib/api'

export default async function handleStatus(accountId: string, setStatus: (status: boolean) => void): Promise<boolean> {
  if (!accountId) return false
  try {
    console.log(accountId)
    const response = await api.patch(`/Account/ChangeActiveStatus/${accountId}`, { accountId })
    if (response.status === 200) {
      setStatus(response.data.isActive)
      return true
    } else {
      return false
    }
  } catch {
    return false
  }
}
