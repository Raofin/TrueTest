'use client'

import { createContext, useContext, useState, useEffect, useCallback } from 'react'
import { useRouter } from 'next/navigation'
import api from '../components/utils/api'
import { getAuthToken, setAuthToken, removeAuthToken } from '@/app/components/utils/auth'

interface User {
  username: string;
  email: string;
}

interface AuthContextType {
  user: User | null;
  login: (email: string, password: string, setError: (error: string) => void) => void
  logout: () => void
}

const AuthContext = createContext<AuthContextType | null>(null)

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const router = useRouter()

  const logout = useCallback(() => {
    removeAuthToken()
    setUser(null)
    router.push('/login')
  }, [router])

  const fetchUser = useCallback(async () => {
    try {
      const response = await api.get('/User/Info')
      setUser(response.data)
    } catch (error) {
      console.error('Failed to fetch user data', error)
      logout()
    }
  }, [logout])
  useEffect(() => {
    const token = getAuthToken()
    if (token) {
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`
      fetchUser()
    }else {
      setUser(null); 
    }
  }, [fetchUser])

  const login = async (email: string, password: string, setError: (error: string) => void) => {
    try {
      const response = await api.post('/Auth/Login', {
        usernameOrEmail: email,
        password: password,
      })
      const { token } = response.data
      setAuthToken(token)
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`
      await fetchUser()
      router.push('/home')
    } catch {
      setError( 'Login failed. Please try again.' )
    }
  }

  return <AuthContext.Provider value={{ user, login, logout }}>{children}</AuthContext.Provider>
}

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}
