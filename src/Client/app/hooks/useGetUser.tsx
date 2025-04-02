'use client'

import { useRouter } from 'next/navigation'
import { useEffect, useState } from 'react'
import api from '@/app/utils/api'

export default function useGetUser() {
  const router = useRouter()
  const [userData, setUserData] = useState<{ roles: string[] } | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await api.get('User/Info')
        if (response.status === 200) setUserData(response.data)
      } catch (err) {
        setError('Failed to fetch user data')
        console.error('Error fetching user info:', err)
      } finally {
        setLoading(false)
      }
    }
    fetchUser()
  }, [router])

  return { userData, loading, error }
}
