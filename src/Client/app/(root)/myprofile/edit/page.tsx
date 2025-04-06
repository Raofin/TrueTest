'use client'

import React, { useState } from 'react'
import { Button, Form } from '@heroui/react'
import ProfileEdit from '@/components/profile/edit/ProfileEdit'
import RootNavBar from '@/app/(root)/root-navbar'
import api from '@/app/utils/api'
import { usePathname, useRouter } from 'next/navigation'
import { FormData } from '@/components/types/profile'
import SweetAlert from '@/components/ui/sweetalert'

export default function MyProfileEdit() {
  const router = useRouter()
  const pathname = usePathname()
  const [formData, setFormData] = useState<FormData>({
    firstName: '',
    lastName: '',
    bio: '',
    instituteName: '',
    phoneNumber: '',
    imageFileId: null,
    profileLinks: [{ name: '', link: '' }],
  })

  const handleProfileUpdate = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      const response = await api.post('/User/SaveProfile', formData)

      if (response.status === 200) {
        <SweetAlert icon="success" text="Profile updated successfully" showConfirmButton={false} timer={1500} />
        const response = await api.get('/User/Info')
        const isAdmin = response.data.roles.some((role: string) => role.toLowerCase() === 'admin')
        router.push(isAdmin ? '/profile' : '/myprofile')
      }
    } catch {
      alert('Profile update failed. Please try again.')
    }
  }
  return (
    <>
      {pathname.includes('/myprofile') && <RootNavBar />}
      <div className="flex justify-center items-center min-h-screen ">
        <Form className=" p-6 rounded-lg shadow-none bg-white dark:bg-[#18181b]" onSubmit={handleProfileUpdate}>
          <h2 className="w-full text-lg font-semibold text-center">Update Profile</h2>
          <ProfileEdit formData={formData} setFormData={setFormData} />
          <div className="w-full mt-5 flex justify-center">
            <Button className=" text-center " color="primary" radius="lg" type="submit">
              Save Changes
            </Button>
          </div>
        </Form>
      </div>
    </>
  )
}
