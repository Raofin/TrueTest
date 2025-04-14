'use client'


import React, { useEffect, useState } from 'react'
import { Button, Card, Input, Textarea, TimeInput } from '@heroui/react'
import { CalendarDate, Time } from '@internationalized/date'
import { DatePicker } from '@heroui/date-picker'
import ProblemSolve from '@/components/ques/problem-solving-ques'
import WrittenQues from '@/components/ques/written-ques'
import McqQues from '@/components/ques/mcq-ques'
import '@/app/globals.css'
import { v4 as uuidv4 } from 'uuid'
import api from '@/utils/api'
import toast from 'react-hot-toast'
import { useRouter, useSearchParams } from 'next/navigation'


interface FormData {
  title: string
  description: string
  durationMinutes: number
  totalPoints: number
  opensAt: string
  closesAt: string
}


function parseTime(time: string): Time | null {
  if (!time) return null
  const [hour, minute] = time.split(':').map(Number)
  return new Time(hour, minute)
}


export default function CreateExamPage() {


  const [date, setDate] = useState<CalendarDate | null>(null)
  const [activeComponents, setActiveComponents] = useState<{ id: string; type: string }[]>([])
  const searchParams = useSearchParams();
  const route=useRouter();
  const [examId,setExamId] =useState(searchParams.get('id')||"");
  const [formData, setFormData] = useState<FormData>({
    title: '',
    description: '',
    durationMinutes: 0,
    totalPoints: 0,
    opensAt: '',
    closesAt: '',
  })
  const handleAddComponent = (componentType: string) => {
    setActiveComponents([...activeComponents, { id: uuidv4(), type: componentType }])
  }
  const handleSaveExam = async (e: React.FormEvent) => {
    e.preventDefault()


    if (!date) {
      toast.error('Please select a date')
      return
    }
    try {
      const formatDateTime = (timeString: string) => {
        if (!timeString || !date) return ''
        const [hours, minutes] = timeString.split(':')
        return new Date(date.year, date.month - 1, date.day, parseInt(hours), parseInt(minutes)).toISOString()
      }


      const examData = {
        ...formData,
        opensAt: formatDateTime(formData.opensAt),
        closesAt: formatDateTime(formData.closesAt),
        date: new Date(date.year, date.month - 1, date.day).toISOString(),
      }


      const response = await api.post('/Exam/Create', examData)


      if (response.status === 200) {
        toast.success('Exam created successfully.')
        setExamId(response.data.examId)
      }
    } catch {}
  }
  useEffect(() => {
    const FetchExamId=async()=>{
      try{
        const response=await api.get(`/Exam/${examId}`)
        if(response.status===200){
          const exam=response.data.exam
            setFormData({
              title: exam.title,
              description: exam.description,
              durationMinutes: exam.durationMinutes,
              totalPoints: exam.totalPoints,
              opensAt: exam.opensAt,
              closesAt: exam.closesAt,
            })
            if (exam.opensAt) {
              const opensAtDate = new Date(exam.opensAt);
              setDate(new CalendarDate(
                opensAtDate.getFullYear(),
                opensAtDate.getMonth() + 1,
                opensAtDate.getDate()
              ));
            }
        }
      }catch{  }
  }
  FetchExamId();
  }, [examId])
 
  const handlePublishExam = async () => {
  if(examId){
    try {
      const response = await api.post(`/Exam/Publish?examId=${examId}`)


      if (response.status === 200) {
        toast.success('Exam published successfully.')
      }
    } catch {
      toast.error('Failed to publish exam')
    }
  }
  }
  const handleDeleteExam = async () => {
   if(examId){
    try {
      const response = await api.delete(`/Exam/Delete/${examId}`)


      if (response.status === 200) {
        toast.success('Exam deleted successfully.')
        setFormData({title: '',
          description: '',
          durationMinutes: 0,
          totalPoints: 0,
          opensAt: '',
          closesAt: '',})
          setExamId('')
          setDate(null)
        route.push('/exams/create')
      }
    } catch {
      toast.error('Failed to delete exam')
    }
   }
  }
  const handleOpenCloseTime = (time: Time | null): string => {
    if (!time) return ''
    return `${String(time.hour).padStart(2, '0')}:${String(time.minute).padStart(2, '0')}`
  }
  return (
    <div className="m-12 flex flex-col gap-8">
      <h1 className="w-full text-center text-3xl font-bold my-3">{`${examId ? 'Edit' : 'Create'} Exam`}</h1>
      <Card className={`flex shadow-none flex-col justify-between p-8 items-center `}>
        <form id="#" className="flex gap-4 flex-wrap flex-col w-full " onSubmit={handleSaveExam}>
          <Input
            className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
            isRequired
            label="Title"
            name="title"
            type="text"
            value={formData.title}
            onChange={(e) => setFormData({ ...formData, title: e.target.value })}
          />
          <Textarea
            className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
            isRequired
            label="Description"
            name="description"
            type="text"
            value={formData.description}
            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
          />
          <div className="flex gap-5">
            <DatePicker
              className="flex-1 bg-[#eeeef0] dark:[#71717a] rounded-2xl"
              isRequired
              label="Exam Date"
              name="date"
              value={date}
              onChange={setDate}
            />
            <Input
              className="flex-1 bg-[#eeeef0] dark:[#71717a] rounded-2xl"
              isRequired
              label="Duration"
              name="duration"
              type="number"
              min="0"
              value={formData.durationMinutes?.toString() ?? "0"}
              onChange={(e) => {
                const value = Number(e.target.value);
                if (!isNaN(value) && value >= 1) {
                  setFormData({ ...formData, durationMinutes: value });
                }
              }}
            />
          </div>
          <div className="flex gap-5">
            <TimeInput
              label="Start Time"
              className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
              name="opensAt"
              value={parseTime(formData.opensAt)}
              isRequired
              onChange={(time) =>
                setFormData({
                  ...formData,
                  opensAt: handleOpenCloseTime(time),
                })
              }
            />
            <TimeInput
              label="End Time"
              className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
              name="closesAt"
              value={parseTime(formData.closesAt)}
              isRequired
              onChange={(time) => setFormData({ ...formData, closesAt: handleOpenCloseTime(time) })}
            />
          </div>
          <div>
            <Input
              className="flex-1 bg-[#eeeef0] dark:[#71717a] rounded-2xl"
              isRequired
              label="Total Points"
              name="totalpoints"
              type="number"
               min="0"
               value={formData.totalPoints?.toString() ?? "0"}
               onChange={(e) => {
                const value = Number(e.target.value);
                if (!isNaN(value) && value >= 1) {
                  setFormData({ ...formData, totalPoints: value });
                }
              }}
            />
          </div>
          <div className="flex justify-end mt-2">
            <Button color="success" onPress={handlePublishExam}>
              Publish
            </Button>
            <Button color="danger" className="mx-3" onPress={handleDeleteExam}>
              Delete
            </Button>
            <Button color="primary" type="submit">
              Save
            </Button>
          </div>
        </form>
      </Card>


      {activeComponents.map((component) => (
        <div key={component.type} className="w-full">
          {component.type === 'problemSolve' && <ProblemSolve examId={examId}/>}
          {component.type === 'writtenQues' && <WrittenQues examId={examId}/>}
          {component.type === 'mcq' && <McqQues examId={examId}/>}
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
