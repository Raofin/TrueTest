'use client'

import React from 'react'
import { Button, Form, Link } from '@heroui/react'
import ProfileEdit from '@/app/components/profile/edit/page'
import { useRouter } from 'next/navigation'
import api from '@/app/utils/api'


export default function ProfileDetails() {
  const router = useRouter()
  const handleRoute = async() => {
    const response=await api.get('/User/Info')
    if(response.status===200){
    const userData=response.data;
    if (!userData) return
    if (userData.roles.some((role:string) => role.toLowerCase() === 'admin')) {
      router.push('/overview')
    } else {
      router.push('/home')
    }
  }
  }
  return (
    <div className="flex justify-center items-center ">
      <Form className=" py-5 px-8 rounded-lg shadow-none bg-white dark:bg-[#18181b]" onSubmit={handleRoute}>
        <h2 className="w-full text-xl font-bold text-center">Add Details</h2>
        <ProfileEdit />
        <div className="w-full flex justify-between mt-6">
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
