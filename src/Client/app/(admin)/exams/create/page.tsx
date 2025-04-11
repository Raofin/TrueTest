'use client'

import React, { useState } from 'react'
import { Button, Card, DatePicker, Input, Textarea, TimeInput } from '@heroui/react'
import { CalendarDate, Time } from '@internationalized/date'
import ProblemSolve from '@/components/ques/problem-solving-ques'
import WrittenQues from '@/components/ques/written-ques'
import McqQues from '@/components/ques/mcq-ques'
import '@/app/globals.css'
import { v4 as uuidv4 } from 'uuid'

interface FormData {
  title: string
  description: string
  durationMinutes: number
  opensAt: string
  closesAt: string
}

function parseTime(time: string): Time | null {
  if (!time) return null
  const [hour, minute] = time.split(':').map(Number)
  return new Time(hour, minute)
}

export default function App() {
  const [date] = React.useState<CalendarDate | null>(null)
  const [activeComponents, setActiveComponents] = useState<{ id: string; type: string }[]>([])

  const handleAddComponent = (componentType: string) => {
    setActiveComponents([...activeComponents, { id: uuidv4(), type: componentType }])
  }

  const [formData] = React.useState<FormData>({
    title: '',
    description: '',
    durationMinutes: 0,
    opensAt: '',
    closesAt: '',
  })

  return (
    <div className="m-12 flex flex-col gap-8">
      <h1 className="w-full text-center text-3xl font-bold my-3">Create (and Edit) Exam</h1>
      <Card className={`flex shadow-none flex-col justify-between p-8 items-center `}>
        <form id="#" className="flex gap-4 flex-wrap flex-col w-full ">
          <Input className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
            isRequired
            label="Title"
            name="title"
            type="text"
            value={formData.title}
          />
          <Textarea className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
            isRequired
            label="Description"
            name="description"
            type="text"
            value={formData.description}
          />
          <div className="flex gap-5">
            <DatePicker
              className="flex-1 bg-[#eeeef0] dark:[#71717a] rounded-2xl"
              isRequired
              label="Date"
              name="date"
              value={date}
            />
            <Input className="flex-1 bg-[#eeeef0] dark:[#71717a] rounded-2xl"
              isRequired
              label="Total Points"
              name="totalpoints"
              type="text"
            />
          </div>
          <div className="flex gap-5">
            <TimeInput
              label="Start Time"
              className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
              name="opensAt"
              value={parseTime(formData.opensAt)}
              isRequired
            />
            <TimeInput
              label="End Time"
              className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
              name="closesAt"
              value={parseTime(formData.closesAt)}
              isRequired
            />
          </div>
          <div className="flex justify-end mt-2">
            <Button color="success">Publish</Button>
            <Button color="danger" className="mx-3">
              Delete
            </Button>
            <Button color="primary" type="submit">
              Save
            </Button>
          </div>
        </form>
      </Card>

      {activeComponents.map((component) => (
        <div key={component.id} className="w-full">
          {component.type === 'problemSolve' && <ProblemSolve />}
          {component.type === 'writtenQues' && <WrittenQues />}
          {component.type === 'mcq' && <McqQues />}
        </div>
      ))}

      <div className="flex gap-3 justify-center my-4">
        {!activeComponents.some((comp) => comp.type === 'problemSolve') && (
          <Button onPress={() => handleAddComponent('problemSolve')}>Add Problem Solving Question</Button>
        )}
        {!activeComponents.some((comp) => comp.type === 'writtenQues') && (
          <Button onPress={() => handleAddComponent('writtenQues')}>Add Written Question</Button>
        )}
        {!activeComponents.some((comp) => comp.type === 'mcq') && (
          <Button onPress={() => handleAddComponent('mcq')}>Add MCQ Question</Button>
        )}
      </div>
    </div>
  )
}
