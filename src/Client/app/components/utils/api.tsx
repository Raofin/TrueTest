import axios from 'axios'
import { getAuthToken, removeAuthToken } from './auth'

const api = axios.create({
  baseURL: 'https://truetest.rawfin.net/api/', 
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
    return Promise.reject(error)
  }
)

api.interceptors.response.use(
  (response) => {
    return response
  },
  async (error) => {
    const originalRequest = error.config

    if (error.response && error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true

      try {
        if (typeof window !== 'undefined') {
          removeAuthToken()
          window.location.href = '/login'
        }
        return Promise.reject(error)
      } catch (refreshError) {
        if (typeof window !== 'undefined') {
          removeAuthToken()
          window.location.href = '/login'
        }
        return Promise.reject(refreshError)
      }
    }
    return Promise.reject(error)
  }
)

export default api
