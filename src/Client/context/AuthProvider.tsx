'use client'

import { createContext, useContext, useState, useEffect, useCallback, useMemo } from 'react'
import { useRouter } from 'next/navigation'
import api from '@/app/utils/api'
import { getAuthToken, setAuthToken, removeAuthToken } from '@/app/utils/auth'
import SweetAlert from '@/components/ui/sweetalert'

interface User {
  username: string
  email: string
  roles: string[]
}

interface AuthContextType {
  user: User | null
  login: (email: string, password: string, setError: (error: string) => void) => void
  logout: () => void
}

const AuthContext = createContext<AuthContextType | null>(null)

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null)
  const router = useRouter()

  const logout = useCallback(() => {
    removeAuthToken()
    setUser(null)
    router.push('/login')
  }, [router])

  const fetchUser = useCallback(async () => {
    const response = await api.get('/User/Info')
    if (response.status === 200) {
      const userData = response.data
      setUser(userData)
    }
  }, [])

  useEffect(() => {
    const token = getAuthToken()
    if (token) {
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`
      fetchUser()
    } else {
      setUser(null)
    }
  }, [fetchUser])

  const login = useCallback(
    async (email: string, password: string, setError: (error: string) => void) => {
      try {
        const response = await api.post('/Auth/Login', {
          usernameOrEmail: email,
          password: password,
        })
        if (response.status === 200) {
        <SweetAlert icon="success" text="logged in successfully" showConfirmButton={false} timer={1500} />
          const { token } = response.data
          setAuthToken(token)
          setUser(response.data)
          api.defaults.headers.common['Authorization'] = `Bearer ${token}`
          await fetchUser()
        }
      } catch {
        setError('Useremail or password invalid. Please try again.')
      }
    },
    [fetchUser]
  )

  const contextValue = useMemo(
    () => ({
      user,
      login,
      logout,
    }),
    [user, login, logout]
  )

  return <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
}

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}
