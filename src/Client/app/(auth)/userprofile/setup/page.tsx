'use client'

import React, { useState } from 'react'
import { Button, Form } from '@heroui/react'
import ProfileEdit from '@/app/components/profile/edit/page'
import { useRouter } from 'next/navigation'
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

export default function ProfileSetUp() {
  const router = useRouter();
  
  const [formData, setFormData] = useState<FormData>({
    firstName: '',
    lastName: '',
    bio: '',
    instituteName: '',
    phoneNumber: '',
    imageFileId: null,
    profileLinks: [{ name: '', link: '' }] 
  });
  
  const handleProfileEdit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await api.post('/User/SaveProfile', formData);

      if (response.status === 200) {
        router.push('/profile');
      }
    } catch {
      alert('Profile update failed. Please try again.');
    }
  };

  const handleSkipButton = async () => {
    try {
      const response = await api.get('/User/Info');
      const isAdmin = response.data.roles.some((role: string) => role.toLowerCase() === 'admin');
      router.push(isAdmin ? '/overview' : '/home');
    } catch(error) {
      alert('Failed to fetch user info. Please try again.');
      console.log(error)
    }
  };

  return (
    <div className="flex justify-center items-center">
      <Form className="py-5 px-8 rounded-lg shadow-none bg-white dark:bg-[#18181b]" onSubmit={handleProfileEdit}>
        <h2 className="w-full text-xl font-bold text-center">Add Details</h2>
        <ProfileEdit formData={formData} setFormData={setFormData} />
        
        <div className="w-full flex justify-between mt-6">
          <Button onPress={handleSkipButton}>
            Skip for now
          </Button>
          <Button color="primary" radius="lg" type="submit">
            Save & Continue
          </Button>
        </div>
      </Form>
    </div>
  );
}