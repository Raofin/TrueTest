'use client'

import React, { useState } from 'react'
import { Button, Input, Form, Card } from '@heroui/react'
import '@/app/globals.css'
import api from '@/app/utils/api'
import { useRouter } from 'next/navigation'
import { Icon } from '@iconify/react/dist/iconify.js'
import Swal from 'sweetalert2'
import axios from 'axios'


export default function Component() {
  const [newconfirmpassword, setNewconfirmpassword] = useState('')
  const [error, setError] = useState('')

  const router = useRouter()
  const [isVisible, setIsVisible] = useState<boolean>(false)
  const [isConfirmVisible, setIsConfirmVisible] = useState<boolean>(false)
  const [isVisibleCurrent, setIsVisibleCurrent] = useState<boolean>(false)
  const [formData, setFormData] = useState({
    username: '',
    currentPassword: '',
    newPassword: '',
  })
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target
    setFormData((prev) => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    if (formData.newPassword !== newconfirmpassword) {
      setError('New password and confirm password do not match.')
      return
    }
    try {
      const response = await api.patch('/User/AccountSettings', formData)
      if (response.status === 200) {
        Swal.fire({
          position: 'center',
          icon: 'success',
          text: 'Account updated successfully',
          showConfirmButton: false,
          timer: 1500,
        })
        if (response.data.roles === 'Admin') router.push('/settings')
        else router.push('/mysettings')
      }
    } catch (error) {
      console.log("Error object:", error);
      if (axios.isAxiosError(error)) {
          console.log("Error response:", error.response);
          setError(error.response?.data?.detail);
      } else {
          setError("username or current password is not correct.");
      }
    }
  }

  return (
    <div className="flex mt-24 w-full items-center justify-center">
      <Card className="flex w-full max-w-sm flex-col gap-4 rounded-large shadow-none bg-white dark:bg-[#18181b] bg-content1 px-8 pb-10 pt-6">
        <div className="flex flex-col gap-3">
          <h1 className="text-2xl font-semibold text-center mb-4">Account Settings</h1>
        </div>
        <Form
          className="flex w-full flex-wrap md:flex-nowrap gap-4 flex-col"
          validationBehavior="native"
          onSubmit={handleSubmit}
        >
          {error && <p className="text-red-500">{error}</p>}
          <Input
            isRequired
            label="Username"
            name="username"
            type="text"
            className="bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl"
            value={formData.username}
            onChange={handleChange}
          />
          <Input
            isRequired
            label="Current Password"
            endContent={
              <button type="button" onClick={() => setIsVisibleCurrent(!isVisibleCurrent)}>
                <Icon
                  className="text-2xl text-default-400"
                  icon={isVisibleCurrent ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                />
              </button>
            }
            name="currentPassword"
            type={isVisibleCurrent ? 'text' : 'password'}
            className="bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl"
            value={formData.currentPassword}
            onChange={handleChange}
          />
          <Input
            isRequired
            endContent={
              <button type="button" onClick={() => setIsVisible(!isVisible)}>
                <Icon
                  className="text-2xl text-default-400"
                  icon={isVisible ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                />
              </button>
            }
            label="New Password"
            name="newPassword"
            type={isVisible ? 'text' : 'password'}
            className="bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl"
            value={formData.newPassword}
            onChange={handleChange}
          />
          <Input
            isRequired
            endContent={
              <button type="button" onClick={() => setIsConfirmVisible(!isConfirmVisible)}>
                <Icon
                  className="text-2xl text-default-400"
                  icon={isConfirmVisible ? 'solar:eye-closed-linear' : 'solar:eye-bold'}
                />
              </button>
            }
            label="Confirm Password"
            name="newconfirmpassword"
            type={isConfirmVisible ? 'text' : 'password'}
            className="bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl"
            value={newconfirmpassword}
            onChange={(e) => setNewconfirmpassword(e.target.value)}
          />
          <Button className="w-full mt-2 text-medium" color="primary" type="submit">
            Save Changes
          </Button>
        </Form>
      </Card>
    </div>
  )
}
