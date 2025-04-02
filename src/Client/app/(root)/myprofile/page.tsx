'use client'

import React from 'react'
import '@/app/globals.css'
import MyProfile from '@/app/components/profile/page'
import RootNavBar from '../root-navbar'


export default function ProfilePage() {

  return (
    <>
    <RootNavBar/>
    <MyProfile/>
    </>
  )
}
