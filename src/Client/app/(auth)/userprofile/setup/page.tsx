'use client'

import React from 'react'
import { Button, Card } from '@heroui/react'
import ProfileEdit from '@/app/components/profile/edit/page'

export default function ProfileDetails() {
  return (
    <>
      <div className="flex justify-center items-center ">
        <Card className=" py-5 px-8 rounded-lg">
          <h2 className="text-xl font-bold text-center">Add Details</h2>
          <ProfileEdit />
          <div className="flex justify-between mt-6">
            <Button>Skip for now</Button>
            <Button color="primary" radius="full" type="submit">
              Save & Continue
            </Button>
          </div>
        </Card>
      </div>
    </>
  )
}
