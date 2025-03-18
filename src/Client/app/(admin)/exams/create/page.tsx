'use client'

import React from 'react'
import { Button, DatePicker, Input, Textarea, TimeInput, useDisclosure } from '@heroui/react'
import { CalendarDate, Time } from '@internationalized/date'
import { Toaster } from 'react-hot-toast'
import CommonModal from '@/app/components/ui/Modal/common-modal'
import ProblemSolve from '@/app/components/ques/problem-solving-ques'
import WrittenQues from '@/app/components/ques/written-ques'
import MCQ from '@/app/components/ques/mcq-ques'
import '@/app/globals.css'
import useTheme from '@/app/hooks/useTheme'

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
  const { isOpen, onOpenChange } = useDisclosure()
  const [activeComponents, setActiveComponents] = React.useState<string[]>([])
  const Mode = useTheme()

  const handleAddComponent = (componentType: string) => {
    if (!activeComponents.includes(componentType)) {
      setActiveComponents([...activeComponents, componentType])
    }
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
      <div
        className={`flex flex-col justify-between p-8 items-center ${Mode === 'dark' ? 'bg-[#18181b]' : 'bg-white'}`}
      >
        <form id="#" className="flex gap-4 flex-wrap flex-col w-full">
          <Input isRequired label="Title" name="title" type="text" variant="bordered" value={formData.title} />
          <Textarea
            isRequired
            label="Description"
            name="description"
            type="text"
            variant="bordered"
            value={formData.description}
          />
          <div className="flex gap-5">
            <DatePicker className="flex-1" isRequired label="Date" name="date" value={date} />
            <Input
              className="flex-1"
              isRequired
              label="Total Points"
              name="totalpoints"
              type="text"
              variant="bordered"
            />
          </div>
          <div className="flex gap-5">
            <TimeInput label="Start Time" name="opensAt" value={parseTime(formData.opensAt)} isRequired />
            <TimeInput label="End Time" name="closesAt" value={parseTime(formData.closesAt)} isRequired />
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
      </div>

      {activeComponents.map((component, index) => (
        <div key={index} className="w-full">
          {component === 'problemSolve' && <ProblemSolve />}
          {component === 'writtenQues' && <WrittenQues />}
          {component === 'mcq' && <MCQ />}
        </div>
      ))}

      <div className="flex gap-3 justify-center mt-4">
        {!activeComponents.includes('problemSolve') && (
          <Button onPress={() => handleAddComponent('problemSolve')}>Add Problem Solving Question</Button>
        )}
        {!activeComponents.includes('writtenQues') && (
          <Button onPress={() => handleAddComponent('writtenQues')}>Add Written Question</Button>
        )}
        {!activeComponents.includes('mcq') && (
          <Button onPress={() => handleAddComponent('mcq')}>Add MCQ Question</Button>
        )}
      </div>
      <CommonModal isOpen={isOpen} onOpenChange={onOpenChange} title="Exam Created Successfully" />
      <Toaster />
    </div>
  )
}
