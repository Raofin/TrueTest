'use client'

import React, { useCallback, useEffect } from 'react'
import { Avatar, Input, Textarea } from '@heroui/react'
import { Icon } from '@iconify/react'
import api from '@/app/utils/api'

interface FormData {
  firstName: string;
  lastName: string;
  bio: string;
  instituteName: string;
  phoneNumber: string;
  imageFileId: File | string | null;
  profileLinks: { name: string; link: string }[];
}

interface ProfileDetailsProps {
  formData: FormData;
  setFormData: React.Dispatch<React.SetStateAction<FormData>>;
}

export default function ProfileEdit({ formData, setFormData }: ProfileDetailsProps) {
  const handleAddSocialLink = () => {
    setFormData(prev => ({
      ...prev,
      profileLinks: [...prev.profileLinks, { name: '', link: '' }]
    }));
  };

  const handleSocialLinkChange = (index: number, field: 'name' | 'link', value: string) => {
    setFormData(prev => {
      const updatedLinks = [...prev.profileLinks];
      updatedLinks[index] = { ...updatedLinks[index], [field]: value };
      return { ...prev, profileLinks: updatedLinks };
    });
  };

  const handleRemoveSocialLink = (index: number) => {
    setFormData(prev => ({...prev,
      profileLinks: prev.profileLinks.filter((_, i) => i !== index)
    }));
  };

  const handleImageUpload = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] || null;
    setFormData(prev => ({ ...prev, imageFileId: file }));
  }, [setFormData]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };
  useEffect(() => {
    const checkProfileStatus = async () => {
      try {
        const response = await api.get('/User/Details')
        if (response.data.profile !== null) {
          setFormData({
            firstName: response.data.profile?.firstName || '',
            lastName: response.data.profile?.lastName || '',
            bio: response.data.profile?.bioMarkdown || '',
            instituteName: response.data.profile?.instituteName || '',
            phoneNumber: response.data.profile?.phoneNumber || '',
            imageFileId: response.data.profile?.imageFileId || null,
            profileLinks: response.data.profile?.profileLinks || [{ name: '', link: '' }]
          })
        }
      } catch (error) {
        console.error('Error checking profile:', error)
      }
    }
    
    checkProfileStatus()
  }, [setFormData])
  return (
    <div className="w-full flex justify-center">
      <div>
        <div className="flex justify-center items-end text-center my-7">
          <div className="relative">
            <Avatar
              className="h-32 w-32"
              size="lg"
              radius="md"
              src={''}
            />
            <label htmlFor="image-upload" className="absolute bottom-0 right-0 rounded-full cursor-pointer">
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
          <div className="flex flex-col gap-2 ml-2">
            <Input 
              name="firstName" 
              placeholder="Enter first name" 
              value={formData.firstName} 
              onChange={handleChange} 
            />
            <Input 
              name="lastName" 
              placeholder="Enter last name" 
              value={formData.lastName} 
              onChange={handleChange} 
            />
          </div>
        </div>
        <div className="w-[550px] grid gap-4">
          <Textarea 
            name="bio" 
            placeholder="Write your bio here." 
            value={formData.bio} 
            rows={5} 
            onChange={handleChange} 
          />
          <div className="grid grid-cols-2 gap-2">
            <Input 
              name="instituteName" 
              placeholder="Enter institute name" 
              value={formData.instituteName} 
              onChange={handleChange} 
            />
            <Input 
              name="phoneNumber" 
              placeholder="Enter phone number" 
              value={formData.phoneNumber} 
              onChange={handleChange} 
            />
          </div>
          <div className="space-y-2">
            {formData.profileLinks.map((link, index) => (
              <div key={index} className="flex gap-2 items-center">
                <Input
                  placeholder="Name"
                  value={link.name}
                  onChange={e => handleSocialLinkChange(index, 'name', e.target.value)}
                />
                <Input
                  placeholder="Social Link"
                  value={link.link}
                  onChange={e => handleSocialLinkChange(index, 'link', e.target.value)}
                />
                {formData.profileLinks.length > 1 && (
                  <button 
                    type="button" 
                    onClick={() => handleRemoveSocialLink(index)}
                  >
                    <Icon icon="solar:trash-bin-trash-bold" width={20} />
                  </button>
                )}
              </div>
            ))}
            <button 
              type="button" 
              onClick={handleAddSocialLink} 
              className="text-blue-500 hover:text-blue-700 flex items-center gap-1"
            >
              <Icon icon="solar:add-circle-bold" width={20} />
              Add Social Link
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}