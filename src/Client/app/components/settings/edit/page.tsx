'use client'

import React, { useState } from 'react'
import { Button, Input, Form, Card } from '@heroui/react'
import '@/app/globals.css'
import api from '@/app/utils/api'

export default function Component() {
  const [newconfirmpassword,setNewconfirmpassword]=useState("")
  const [formData, setFormData] = useState({
    username: '',
    password: '',
    newpassword: '',
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (formData.newpassword !== newconfirmpassword) {
      alert("New password and confirm password do not match.");
      return;
    }
    try {
      const response = await api.patch('/User/AccountSettings', formData);
      if (response.status === 200) {
        alert("Settings updated successfully!");
      }
    } catch (error) {
      console.error("An error occurred:", error);
      alert("An unexpected error has occurred");
    }
  };

  return (
    <div className="flex mt-24 w-full items-center justify-center">
      <Card className="flex w-full max-w-sm flex-col gap-4 rounded-large shadow-none bg-white dark:bg-[#18181b] bg-content1 px-8 pb-10 pt-6">
        <div className="flex flex-col gap-3">
          <h1 className="text-2xl font-semibold text-center mb-4">Account Settings</h1>
        </div>
        <Form
          className="flex w-full flex-wrap md:flex-nowrap gap-4 flex-col"
          validationBehavior="native"
          onSubmit={handleSubmit}
        >
          <Input
            isRequired
            label="Username"
            name="username"
            type="text"
            className="bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl"
            value={formData.username}
            onChange={handleChange}
          />
          <Input
            isRequired
            label="Current Password"
            name="password"
            type="password"
            className="bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl"
            value={formData.password}
            onChange={handleChange}
          />
          <Input
            isRequired
            label="New Password"
            name="newpassword"
            type="password"
            className="bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl"
            value={formData.newpassword}
            onChange={handleChange}
          />
          <Input
            isRequired
            label="Confirm Password"
            name="newconfirmpassword"
            type="password"
            className="bg-[#f4f4f5] dark:bg-[#27272a] rounded-xl"
            value={newconfirmpassword} 
            onChange={()=>setNewconfirmpassword(newconfirmpassword)} 
          />
          <Button className="w-full mt-2 text-medium" color="primary" type="submit">
            Save Changes
          </Button>
        </Form>
      </Card>
    </div>
  );
}
