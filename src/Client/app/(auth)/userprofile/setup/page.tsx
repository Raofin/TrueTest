'use client'

import React from 'react'
import { Button, Form, Link } from '@heroui/react'
import ProfileEdit from '@/app/components/profile/edit/page'
import { useRouter } from 'next/navigation'
import useGetUser from '@/app/hooks/useGetUser'

export default function ProfileDetails() {
  const router = useRouter()
  const { userData } = useGetUser()
  const handleRoute = () => {
    if (!userData) return
    if (userData.roles.some((role) => role.toLowerCase() === 'admin')) {
      router.push('/overview')
    } else {
      router.push('/home')
    }
  }
  return (
    <div className="flex justify-center items-center ">
      <Form className=" py-5 px-8 rounded-lg shadow-none bg-white dark:bg-[#18181b]" onSubmit={handleRoute}>
        <h2 className="text-xl font-bold text-center">Add Details</h2>
        <ProfileEdit />
        <div className="flex justify-between mt-6">
          <Button type="submit">
            <Link href="/home" className="text-[#3f3f46] dark:text-white">
              Skip for now
            </Link>
          </Button>
          <Button color="primary" radius="full" type="submit">
            Save & Continue
          </Button>
        </div>
      </Form>
    </div>
  )
}
