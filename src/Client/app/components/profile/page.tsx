'use client'

import React, { useEffect, useState } from 'react'
import { Avatar, Card, Link, Button } from '@heroui/react'
import { FaLink } from 'react-icons/fa6'
import { usePathname } from 'next/navigation'
import api from '@/app/utils/api'
import FormattedDate from '../format-date-time'

interface ProfileLink {
  profileListId: string
  name: string
  link: string
}

interface UserProfile {
  profileId: string
  firstName: string
  lastName: string
  bioMarkdown?: string
  instituteName?: string
  phoneNumber?: string
  imageFileId: string | null
  profileList?: ProfileLink[] 
}

interface User {
  accountId: string
  username: string
  email: string
  createdAt: string
  isActive: boolean
  profile: UserProfile | null
  roles: string[]
}

export default function ProfilePage() {
  const path = usePathname()
  const [route, setRoute] = useState('')
  const [userInfo, setUserInfo] = useState<User | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    setRoute(path.startsWith('/profile') ? 'profile' : 'myprofile')
  }, [path])

  useEffect(() => {
    const fetchProfileInfo = async () => {
      try {
        setLoading(true)
        const response = await api.get('/User/Details')
        if (response.status === 200) {
          const normalizedData = {
            ...response.data,
            profile: response.data.profile ? {
              ...response.data.profile,
              profileList: response.data.profile.profileList || [] 
            } : null
          }
          setUserInfo(normalizedData)
        }
      } catch (error) {
        console.error('Error fetching profile:', error)
        setUserInfo({
          accountId: '',
          username: '',
          email: '',
          createdAt: new Date().toISOString(),
          isActive: false,
          roles: [],
          profile: null
        })
      } finally {
        setLoading(false)
      }
    }
    fetchProfileInfo()
  }, [])

  if (loading) {
    return (
      <div className="h-full flex items-center justify-center mt-5">
        <Card className="p-8 w-[600px] text-center">Loading profile...</Card>
      </div>
    )
  }

  return (
    <div className="h-full flex items-center justify-center mt-5">
      <Card className="relative rounded-2xl p-8 w-[600px] overflow-visible shadow-none bg-white dark:bg-[#18181b]">
        <div className="flex items-center mb-8">
          <Avatar
            src={userInfo?.profile?.imageFileId || ''}
            alt="Profile"
            radius="md"
            className="absolute w-36 h-36 -mt-20 ml-24"
          />
          <div className="ml-64">
            <h2 className="text-3xl font-semibold">
              {userInfo?.profile?.firstName || 'No first name'} {userInfo?.profile?.lastName || 'No last name'}
            </h2>
            <p className="text-gray-400">@{userInfo?.username || 'username not provided'}</p>
          </div>
        </div>
        {userInfo?.profile ? (
          <>
            <p className="text-md mb-4">{userInfo.profile.bioMarkdown || 'No bio provided'}</p>
            <hr />
            <div className="space-y-2 mb-4 mt-4">
              <p className="text-md flex gap-3">
                <strong>Email:</strong>
                <Link href={`mailto:${userInfo.email}`} className="text-[#71717a] dark:text-white">
                  {userInfo.email}
                </Link>
              </p>
              {userInfo.profile.instituteName && (
                <p className="text-md flex gap-3">
                  <strong>Institute:</strong>
                  <span className="text-[#71717a] dark:text-white">
                    {userInfo.profile.instituteName}
                  </span>
                </p>
              )}
              {userInfo.profile.phoneNumber && (
                <p className="text-md flex gap-3">
                  <strong>Phone:</strong>
                  <span className="text-[#71717a] dark:text-white">
                    {userInfo.profile.phoneNumber}
                  </span>
                </p>
              )}
              {userInfo.profile.profileList && userInfo.profile.profileList.length > 0 && (
                <p className="text-md flex gap-3 items-center">
                  <strong>Links:</strong>
                  <span className="flex flex-wrap gap-3">
                    {userInfo.profile.profileList.map((link) => (
                      <Link
                        key={link.profileListId}
                        href={link.link}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="flex items-center gap-2 text-[#71717a] dark:text-white"
                      >
                        {link.name} <FaLink />
                      </Link>
                    ))}
                  </span>
                </p>
              )}
              <div className="text-md flex gap-3">
                <strong>Joined:</strong>
                <span className="text-[#71717a] dark:text-white">
                  <FormattedDate date={userInfo.createdAt} />
                </span>
              </div>
            </div>
          </>
        ) : (
          <div className="text-center py-8">
            <p className="text-lg mb-4">You haven&apos;t set up your profile yet</p>
          </div>
        )}
        <div className="flex justify-center">
          <Button
            color="primary"
            className="font-semibold py-2 px-4 rounded-lg"
            as={Link}
            href={`/${route}/edit`}
          >
            {userInfo?.profile ? 'Update Profile' : 'Create Profile'}
          </Button>
        </div>
      </Card>
    </div>
  )
}