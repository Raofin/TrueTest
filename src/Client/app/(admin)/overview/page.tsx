'use client'

import React from 'react'
import { Card, CardHeader, CardBody } from '@heroui/react'
import { FaBookOpen, FaCheckCircle, FaThList, FaUser, FaUsers } from 'react-icons/fa'
import { AiFillPieChart } from 'react-icons/ai'


export default function Component() {
  const stats = [
    { icon: <FaUser size={70} />, value: 1424, title: 'Total Users', subtitle: 'in this platform' },
    { icon: <FaUsers size={70} />, value: 245, title: 'New Users', subtitle: 'registered this month' },
    { icon: <FaBookOpen size={70} />, value: 425, title: 'Total Exams', subtitle: 'created in this platform' },
    { icon: <FaThList size={70} />, value: 422, title: 'Total Questions', subtitle: 'created in this platform' },
    { icon: <FaCheckCircle size={70} />, value: 162, title: 'Total Submissions', subtitle: 'attempted by candidates' },
    { icon: <AiFillPieChart size={70} />, value: 75, title: 'Average Score', subtitle: 'across all candidates' },
  ]
  return (
    <div className={`flex h-screen flex-wrap gap-6 w-full items-center justify-center`}>
      <h1 className='w-full text-center text-3xl font-bold my-3'>Overview</h1>
      {stats.map((stat, index) => (
        <Card key={index} className={`py-4 w-[300px] text-center shadow-none`}>
          <CardHeader className="pb-0 pt-2 px-4 flex flex-col items-center">
            {stat.icon}
            <h1 className="font-bold text-5xl">{stat.value}</h1>
          </CardHeader>
          <CardBody className="py-2 mt-4 text-center">
            <h2 className="text-lg font-semibold">{stat.title}</h2>
            <p className="text-sm text-gray-400">{stat.subtitle}</p>
          </CardBody>
        </Card>
      ))}
    </div>
  )
}
