'use client'

import React, { useEffect, ComponentType, useState } from 'react'
import { useRouter } from 'next/navigation'
import { getAuthToken } from '@/lib/auth'

function withProtectedRoute<P extends object>(WrappedComponent: ComponentType<P>): ComponentType<P> {
  const ProtectedComponent: ComponentType<P> = (props: P) => {
    const router = useRouter()
    const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null)
    useEffect(() => {
      const checkAuth = async () => {
        const token = getAuthToken()
        if (!token) {
          router.replace('/')
          setIsAuthenticated(false)
        } else {
          setIsAuthenticated(true)
        }
      }
      checkAuth()
    }, [router])

    if (isAuthenticated === null) {
      return <div>Loading...</div>
    }

    if (isAuthenticated) {
      return <WrappedComponent {...props} />
    }

    return null
  }

  return ProtectedComponent
}

export default withProtectedRoute
