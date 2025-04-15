'use client'

import React from 'react'
import { Card, CardHeader, CardBody } from '@heroui/react'
import { FaBookOpen, FaCheckCircle, FaThList, FaUser, FaUsers } from 'react-icons/fa'
import { AiFillPieChart } from 'react-icons/ai'


export default function Component() {
  const stats = [
    {id:'1', icon: <FaUser size={70} />, value: 1424, title: 'Total Users', subtitle: 'in this platform' },
    {id:'2', icon: <FaUsers size={70} />, value: 245, title: 'New Users', subtitle: 'registered this month' },
    {id:'3', icon: <FaBookOpen size={70} />, value: 425, title: 'Total Exams', subtitle: 'created in this platform' },
    {id:'4', icon: <FaThList size={70} />, value: 422, title: 'Total Questions', subtitle: 'created in this platform' },
    {id:'5', icon: <FaCheckCircle size={70} />, value: 162, title: 'Total Submissions', subtitle: 'attempted by candidates' },
    {id:'6', icon: <AiFillPieChart size={70} />, value: 75, title: 'Average Score', subtitle: 'across all candidates' },
  ]
  return (
    <div >
      <h1 className='w-full text-center text-3xl font-bold my-5'>Overview</h1>
      <div className="h-screen flex flex-col items-center justify-center">
    <div className='flex flex-wrap max-w-[1200px] gap-5 w-full justify-center items-center'>
    {stats.map((stat) => (
        <Card key={stat.id} className={`py-4 w-[300px] text-center shadow-none`}>
          <CardHeader className="pb-0 pt-2 px-4 flex flex-col items-center">
            {stat.icon}
            <h1 className="font-bold text-5xl">{stat.value}</h1>
          </CardHeader>
          <CardBody className="py-2 mt-4 text-center">
            <h2 className="text-lg font-semibold">{stat.title}</h2>
            <p className="text-sm text-gray-400">{stat.subtitle}</p>
          </CardBody>
        </Card>
      ))}</div>
    </div>
    </div>
  )
}
