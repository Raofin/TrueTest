import axios from 'axios'
import { getAuthToken, removeAuthToken } from './auth'

const api = axios.create({
  baseURL: `${process.env.NEXT_PUBLIC_TRUETEST_URL}/api`,
  headers: {
    'Content-Type': 'application/json',
  },
})

api.interceptors.request.use(
  (config) => {
    if (typeof window !== 'undefined') {
      const token = getAuthToken()
      if (token) {
        config.headers.Authorization = `Bearer ${token}`
      }
    }
    return config
  },
  (error) => {
    return Promise.reject(new Error(error.message || 'Request failed'))
  }
)

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true

      try {
        if (typeof window !== 'undefined') {
          removeAuthToken()
          window.location.href = '/signin'
        }
        return Promise.reject(new Error('Session expired. Please log in again.'))
      } catch {
        if (typeof window !== 'undefined') {
          removeAuthToken()
          window.location.href = '/signin'
        }
        return Promise.reject(new Error('Authentication failed. Please log in again.'))
      }
    }
    const errorMessage = error.response?.data?.message || error.message || 'An unexpected error occurred'
    return Promise.reject(new Error(errorMessage))
  }
)

export default api
