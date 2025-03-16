'use client'

import React, { useState, useCallback } from 'react'
import { Card, CardHeader, CardBody, Button, Avatar, Input, Textarea, CardFooter } from '@heroui/react'
import { Icon } from '@iconify/react'
import 'app/globals.css'

export default function ProfileDetails() {
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [bioMarkdown, setBioMarkdown] = useState('')
  const [instituteName, setInstituteName] = useState('')
  const [phoneNumber, setPhoneNumber] = useState('')
  const [imageFileId, setImageFileId] = useState('')
  const [socialLinks, setSocialLinks] = useState<string[]>([''])
  const [loading] = useState(false)

  const handleAddSocialLink = () => {
    setSocialLinks([...socialLinks, ''])
  }

  const handleSocialLinkChange = (index: number, value: string) => {
    const newLinks = [...socialLinks]
    newLinks[index] = value
    setSocialLinks(newLinks)
  }

  const handleImageUpload = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0]
    if (!file) return
    setImageFileId('some-image-file-id')
  }, [])

  return (
    <>
      <div className="flex justify-center items-center min-h-screen ">
        <Card className="w-[800px] p-6 shadow-lg rounded-lg">
          <form id='#'>
            <h2 className="text-lg font-semibold my-3 text-center">Add Details</h2>
            <CardHeader className="flex items-center text-center">
              <div className="relative">
                <Avatar
                  className="h-24 w-24 ml-56"
                  size="lg"
                  src={imageFileId ? `your-image-server/${imageFileId}` : ''}
                />
                <label htmlFor="image-upload" className="absolute bottom-2 right-2 rounded-full shadow cursor-pointer">
                  <Icon icon="solar:pen-2-linear" width={20} />
                  <input
                    id="image-upload"
                    type="file"
                    accept="image/*"
                    className="hidden"
                    onChange={handleImageUpload}
                  />
                </label>
              </div>
              <div className="flex flex-col gap-3 ml-4">
                <Input
                  label=""
                  placeholder="Enter first name"
                  value={firstName}
                  onChange={(e) => setFirstName(e.target.value)}
                />
                <Input
                  label=""
                  placeholder="Enter last name"
                  value={lastName}
                  onChange={(e) => setLastName(e.target.value)}
                />
              </div>
            </CardHeader>
            <CardBody className="grid gap-4">
              <Textarea
                label=""
                placeholder="Write your bio here."
                value={bioMarkdown}
                onChange={(e) => setBioMarkdown(e.target.value)}
              />
              <div className="grid grid-cols-2 gap-2">
                <Input
                  label=""
                  placeholder="Enter institute name"
                  value={instituteName}
                  onChange={(e) => setInstituteName(e.target.value)}
                />
                <Input
                  label=""
                  placeholder="Enter phone number"
                  value={phoneNumber}
                  onChange={(e) => setPhoneNumber(e.target.value)}
                />
              </div>
              <div className="grid grid-cols-2 gap-2">
                {socialLinks.map((link, index) => (
                  <div key={index} className="flex items-center gap-2">
                    <Input
                      label={`Social Link ${index + 1}`}
                      placeholder="Enter social link"
                      value={link}
                      onChange={(e) => handleSocialLinkChange(index, e.target.value)}
                    />
                  </div>
                ))}
              </div>
              <div className="flex justify-start mt-2">
                <button
                  type="button"
                  onClick={handleAddSocialLink}
                  className="flex items-center text-blue-500 hover:text-blue-700 transition-colors"
                >
                  <Icon icon="lucide:circle-plus" className="w-6 h-6 mr-2" />
                  Add Social Link
                </button>
              </div>
            </CardBody>
            <CardFooter className="flex justify-center">
              <Button color="primary" radius="full" type="submit" isLoading={loading}>
                Save & Continue
              </Button>
            </CardFooter>
          </form>
        </Card>
      </div>
    </>
  )
}
