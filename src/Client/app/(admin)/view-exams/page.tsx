'use client'

import React, { useEffect, useState } from 'react'
import { Button, Card, CardBody, CardHeader } from '@heroui/react'
import PaginationButtons from '@/components/ui/pagination-button'
import CommonModal from '@/components/ui/Modal/edit-delete-modal'
import api from '@/lib/api'
import toast from 'react-hot-toast'
import {FormattedDateWeekday, convertUtcToLocalTime,FormatTimeHourMinutes} from '@/components/format-date-time'
import {useRouter } from 'next/navigation'

interface Exam {
  examId: string
  title: string
  description: string
  durationMinutes: number
  opensAt: string
  createdAt: string
  closesAt: string
  totalPoints: number
  problemSolvingPoints: number
  writtenPoints: number
  mcqPoints: number
  status: 'Running' | 'Scheduled' | 'Ended'
  acceptedCandidates: number 
  problemSolving: number
  written: number
  mcq: number
}


const ITEMS_PER_PAGE = 3

export default function ViewExam() {
  const [currentPage, setCurrentPage] = useState(1)
  const [allExam, setAllExam] = useState<Exam[]>([])
  const router=useRouter();
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const totalPages = Math.max(1, Math.ceil(allExam.length / ITEMS_PER_PAGE))
  useEffect(() => {
    const handleViewExam = async () => {
      try {
        const response = await api.get('/Exam')
        if (response.status === 200) {
          setAllExam(response.data)
        }
      } catch {
        toast.error('An error has occurered')
      }
    }
    handleViewExam()
  }, [])
  useEffect(() => {
    setCurrentPage((prev) => Math.min(prev, totalPages))
    if (currentPage > totalPages) {
      setCurrentPage(totalPages > 0 ? totalPages : 1)
    }
  }, [currentPage, totalPages])


  const paginatedExams = allExam.slice((currentPage - 1) * ITEMS_PER_PAGE, currentPage * ITEMS_PER_PAGE)
  const handleEdit = (exam: Exam) => {
    router.push(`/exams/edit?id=${exam.examId}&isEdit=true`)
  }
const handleReview=(exam:Exam)=>{
   router.push(`/exams/review-results?examId=${exam.examId}?examTitle=${exam.title}`)
}

  return (
    <div className='h-full flex flex-col justify-between'>
        <h1 className="w-full text-center text-3xl font-bold my-3">Exams</h1>
      <div className='mt-8 w-full flex flex-col gap-5 items-center justify-center'>
        {paginatedExams.map((exam) => (
          <Card key={exam.examId} className="w-[1000px] shadow-none p-2">
            <CardHeader className="flex justify-between items-center ">
              <div className="flex items-end gap-1">
                <h1 className="text-2xl font-bold flex gap-1 items-end text-[#3f3f46] dark:text-white">{exam.title}</h1>
                {(() => {
                  if (exam.status === 'Scheduled') {
                    return <p className="text-blue-500">{exam.status}</p>
                  } else if (exam.status === 'Running') {
                    return <p className="text-red-500">{exam.status}</p>
                  } else {
                    return <p className='text-gray-500'>{exam.status}</p>
                  }
                })()}
              </div>
              <div className="flex gap-2">
                {(() => {
                  if (exam.status === 'Ended') {
                    return (
                      <Button color="primary" onPress={()=>handleReview(exam)}>Review</Button>
                    )
                  } else if (exam.status === 'Scheduled') {
                    return (
                      <>
                        <Button onPress={() => handleEdit(exam)}>Edit</Button>
                        <Button color="primary">Publish</Button>
                      </>
                    )
                  } else {
                    return <Button onPress={() => handleEdit(exam)}>Edit</Button>
                  }
                })()}
              </div>
            </CardHeader>
            <CardBody className="px-3">
              <div className="flex">
                <div className="flex flex-col flex-1">
                  <p>
                    <span className="text-[#71717a] dark:text-white">Date: </span><FormattedDateWeekday date={exam.opensAt}/>
                  </p>
                  <p>
                    <span className="text-[#71717a] dark:text-white">Duration: </span><FormatTimeHourMinutes minute={exam.durationMinutes}/> hr
                  </p>
                  <p>
                    <span className="text-[#71717a] dark:text-white">Starts at: </span>{convertUtcToLocalTime(exam.opensAt)}
                  </p>
                  <p>
                    <span className="text-[#71717a] dark:text-white">Closes at: </span>{convertUtcToLocalTime(exam.closesAt)}
                  </p>
                </div>
                <div className="flex flex-col flex-1">
                  <p>
                    <span className="text-[#71717a] dark:text-white">Problem Solving: </span>{exam.problemSolvingPoints}
                  </p>
                  <p>
                    <span className="text-[#71717a] dark:text-white">Written: </span>{exam.writtenPoints}
                  </p>
                  <p>
                    <span className="text-[#71717a] dark:text-white">MCQ: </span>{exam.mcqPoints}
                  </p>
                  <p>
                    <span className="text-[#71717a] dark:text-white">Score: </span> {exam.totalPoints}
                  </p>
                </div>
                
              </div>
            </CardBody>
          </Card>
        ))}
      </div>
        <div className="flex w-full justify-center items-center py-5">
          <span className="mx-4">
            Page {currentPage} of {totalPages}
          </span>
          <PaginationButtons
            currentIndex={currentPage}
            totalItems={totalPages}
            onPrevious={() => setCurrentPage((prev) => Math.max(1, prev - 1))}
            onNext={() => setCurrentPage((prev) => Math.min(totalPages, prev + 1))}
          />
        </div>
      <CommonModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        title="Delete Confirmation"
        content="Do you want to delete this user record?"
        confirmButtonText="Delete"
        onConfirm={() => setIsDeleteModalOpen(false)}
      />
    </div>
  )
}
