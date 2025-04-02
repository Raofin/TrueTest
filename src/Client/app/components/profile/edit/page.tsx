'use client'

import React, { useState, useCallback } from 'react'
import { Avatar, Input, Textarea } from '@heroui/react'
import { Icon } from '@iconify/react'
import { v4 as uuidv4 } from 'uuid'

export default function ProfileDetails() {
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')
  const [bioMarkdown, setBioMarkdown] = useState('')
  const [instituteName, setInstituteName] = useState('')
  const [phoneNumber, setPhoneNumber] = useState('')
  const [imageFileId, setImageFileId] = useState('')
  const [socialLinks, setSocialLinks] = useState([{ id: uuidv4(), url: '' }])

  const handleAddSocialLink = () => {
    setSocialLinks([...socialLinks, { id: uuidv4(), url: '' }])
  }
  const handleSocialLinkChange = (id: string, value: string) => {
    setSocialLinks(socialLinks.map((link) => (link.id === id ? { ...link, url: value } : link)))
  }
  const handleImageUpload = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0]
    if (!file) return
    setImageFileId('some-image-file-id')
  }, [])
 
  return (
    <div className="w-full flex justify-center">
      <div id="#">
        <div className="flex justify-center items-end text-center my-7">
          <div className="relative">
            <Avatar
              className="h-32 w-32"
              size="lg"
              radius="md"
              src={imageFileId ? `your-image-server/${imageFileId}` : ''}
            />
            <label htmlFor="image-upload" className="absolute bottom-0 right-0 rounded-full cursor-pointer">
              <Icon icon="solar:pen-2-linear" width={20} />
              <input id="image-upload" type="file" accept="image/*" className="hidden" onChange={handleImageUpload} />
            </label>
          </div>
          <div className="flex flex-col gap-2 ml-2">
            <Input
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              placeholder="Enter first name"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
            />
            <Input
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              placeholder="Enter last name"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
            />
          </div>
        </div>
        <div className="w-[550px] grid gap-4">
          <Textarea
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
            placeholder="Write your bio here."
            value={bioMarkdown}
            rows={5}
            onChange={(e) => setBioMarkdown(e.target.value)}
          />
          <div className="grid grid-cols-2 gap-2">
            <Input
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              label="Enter institute name"
              value={instituteName}
              onChange={(e) => setInstituteName(e.target.value)}
            />
            <Input
              className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
              label="Enter phone number"
              value={phoneNumber}
              onChange={(e) => setPhoneNumber(e.target.value)}
            />
          </div>
          <div className="space-y-2">
            {socialLinks.map((link) => (
              <div key={link.id} className="flex gap-2 items-center">
                <Input
                  label="Social Link"
                  value={link.url}
                  onChange={(e) => handleSocialLinkChange(link.id, e.target.value)}
                />
                <button
                  type="button"
                  onClick={handleAddSocialLink}
                  className="flex items-center text-blue-500 hover:text-blue-700 transition-colors"
                >
                  <Icon icon="lucide:circle-plus" className="w-6 h-6 mr-2" />
                </button>
              </div>
            ))}
          </div>
          <div className="flex justify-start mt-2"></div>
        </div>
      </div>
    </div>
  )
}
