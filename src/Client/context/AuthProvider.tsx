'use client'

import { createContext, useContext, useState, useEffect, useCallback, useMemo } from 'react'
import { useRouter } from 'next/navigation'
import api from '@/lib/api'
import { getAuthToken, setAuthToken, removeAuthToken } from '@/lib/auth'
import ROUTES from '@/constants/route'

interface User {
  username: string
  email: string
  roles: string[]
}

interface AuthContextType {
  user: User | null
  login: (email: string, password: string, setError: (error: string) => void, rememberMe: boolean) => void
  logout: () => void
  fetchUser: () => Promise<User | null>
}

const AuthContext = createContext<AuthContextType | null>(null)

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null)
  const router = useRouter()

  const logout = useCallback(() => {
    removeAuthToken()
    setUser(null)
    router.push(ROUTES.SIGN_IN)
  }, [router])

  const fetchUser = useCallback(async (): Promise<User | null> => {
    try {
      const response = await api.get(ROUTES.USER_INFO)
      if (response.status === 200) {
        const userData: User = response.data
        setUser(userData)
        return userData
      }
    } catch (error) {
      console.error('Error fetching user:', error)
    }
    return null
  }, [])
  useEffect(() => {
    const token = getAuthToken();
    if (token) {
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      fetchUser().then((fetchedUser) => {
        if (fetchedUser) {
          if (fetchedUser.roles.includes('Admin')) {
            router.push(ROUTES.OVERVIEW);
            return
          } else {
            router.push(ROUTES.HOME);
            return
          }
        }
      });
    } else {
      setUser(null);
        router.push(ROUTES.SIGN_IN);
    }
  }, []);

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
    async (usernameOrEmail: string, password: string, setError: (error: string) => void, rememberMe: boolean) => {
      try {
        const response = await api.post('/Auth/Login', {
          usernameOrEmail: usernameOrEmail,
          password: password,
        })
        if (response.status === 200) {
          const { token } = response.data
          setAuthToken(token, rememberMe)
          setUser(response.data)
          api.defaults.headers.common['Authorization'] = `Bearer ${token}`
          const fetchedUser = await fetchUser()
          if (fetchedUser) {
            if (fetchedUser.roles.includes('Admin')) {
              router.push(ROUTES.OVERVIEW)
            } else {
              router.push(ROUTES.HOME)
            }
          }
        } else {
          setError('Useremail or password invalid. Please try again.')
        }
      } catch {
        setError('Useremail or password invalid. Please try again.')
      }
    },
    [fetchUser, router]
  )

  const contextValue = useMemo(
    () => ({
      user,
      login,
      logout,
      fetchUser,
    }),
    [user, login, logout, fetchUser]
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
