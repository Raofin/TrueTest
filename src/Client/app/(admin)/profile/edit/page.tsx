'use client'

import { Button, Card } from '@heroui/react'
import ProfileEdit from '@/app/components/profile/edit/ProfileEdit'

export default function Component() {
  return (
    <div className="flex justify-center items-center min-h-screen ">
      <Card className=" p-6 rounded-lg">
        <h2 className="text-lg font-semibold text-center">Update Profile</h2>
        <ProfileEdit />
        <div className="flex justify-center">
          <Button color="primary" radius="lg" type="submit">
            Save Changes
          </Button>
        </div>
      </Card>
    </div>
  )
}
