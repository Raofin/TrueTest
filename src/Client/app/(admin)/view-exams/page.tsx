'use client'

import React, { useEffect, useState } from 'react'
import { Button, Card, CardBody, CardHeader, Link } from '@heroui/react'

import PaginationButtons from '@/components/ui/pagination-button'
import CommonModal from '@/components/ui/Modal/edit-delete-modal'
import api from '@/utils/api'
import toast from 'react-hot-toast'
import {FormattedDateWeekday, FormattedTime} from '@/components/format-date-time'
import { useRouter } from 'next/navigation'
import FormatTimeHourMinutes from '@/components/format-time'

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
  status: 'Published' | 'Draft' | 'Ended'
  invitedCandidates: number | 'N/A'
  acceptedCandidates: number | 'N/A'
  problemSolving: number
  written: number
  mcq: number
  score: number
}

const ITEMS_PER_PAGE = 2

export default function ExamList({invitedCandidates}:{invitedCandidates:number}) {
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
    router.push(`/exams/create?id=${exam.examId}`)
  }

  return (
    <>
      <div className={`h-screen flex flex-col justify-between w-full`}>
        <h1 className="w-full text-center text-3xl font-bold my-3">Exams</h1>
        {paginatedExams.map((exam) => (
          <Card key={exam.examId} className="mx-8 p-4 shadow-none">
            <CardHeader className="flex justify-between items-center ">
              <div className="flex items-end gap-1">
                <h1 className="text-2xl font-bold flex gap-1 items-end text-[#3f3f46] dark:text-white">{exam.title}</h1>
                {(() => {
                  if (exam.status === 'Published') {
                    return <p className="text-green-500">{exam.status}</p>
                  } else if (exam.status === 'Draft') {
                    return <p className="text-yellow-500">{exam.status}</p>
                  } else {
                    return <p className="text-blue-600">{exam.status}</p>
                  }
                })()}
              </div>
              <div className="flex gap-2">
                {(() => {
                  if (exam.status === 'Ended') {
                    return (
                      <Button color="primary">
                        <Link href="/exams/review-results" className={'text-white'}>
                          Review Results
                        </Link>
                      </Button>
                    )
                  } else if (exam.status === 'Draft') {
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
                    <span className="text-[#71717a] dark:text-white">Starts at: </span><FormattedTime date={exam.opensAt}/>
                  </p>
                  <p>
                    <span className="text-[#71717a] dark:text-white">Closes at: </span><FormattedTime date={exam.closesAt}/>
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
                    <span className="text-[#71717a] dark:text-white">Score: </span> {exam.score}
                  </p>
                </div>
                <div className="flex flex-col flex-1">
                  <p>
                    <span className="text-[#71717a] dark:text-white">Invited Candidates: </span> {invitedCandidates}
                  </p>
                  <p>
                    <span className="text-[#71717a] dark:text-white">Accepted: </span> {exam.acceptedCandidates}
                  </p>
                </div>
              </div>
            </CardBody>
          </Card>
        ))}
        <div className="flex justify-center items-center my-4">
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
      </div>
      <CommonModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        title="Delete Confirmation"
        content="Do you want to delete this user record?"
        confirmButtonText="Delete"
        onConfirm={() => setIsDeleteModalOpen(false)}
      />
    </>
  )
}
