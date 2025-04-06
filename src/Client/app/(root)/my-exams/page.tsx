'use client'

import React, { useState } from 'react'
import { Button, Card, CardBody, CardHeader, Link } from '@heroui/react'
import PaginationButtons from '@/components/ui/pagination-button'
import RootNavBar from '../root-navbar'

interface Exam {
  id: number
  title: string
  description: string
  durationMinutes: number
  opensAt: string
  closesAt: string
  status: 'Running' | 'Upcoming' | 'Ended'
  problemSolving: number
  written: number
  mcq: number
  score: number
}

const Exams: Exam[] = [
  {
    id: 1,
    title: 'Star Coder 2026',
    description: 'Running',
    durationMinutes: 60,
    opensAt: '2026-11-21T21:00:00.000Z',
    closesAt: '2026-11-21T22:20:00.000Z',
    status: 'Running',
    problemSolving: 10,
    written: 10,
    mcq: 30,
    score: 100,
  },
  {
    id: 2,
    title: 'Learnathon 4.0',
    description: 'Upcoming',
    durationMinutes: 90,
    opensAt: '2026-12-10T21:00:00.000Z',
    closesAt: '2026-12-10T23:00:00.000Z',
    status: 'Upcoming',
    problemSolving: 10,
    written: 10,
    mcq: 30,
    score: 100,
  },
  {
    id: 3,
    title: 'Star Coder 2005',
    description: 'Ended',
    durationMinutes: 60,
    opensAt: '2025-11-21T21:00:00.000Z',
    closesAt: '2025-11-21T22:00:00.000Z',
    status: 'Ended',
    problemSolving: 10,
    written: 10,
    mcq: 30,
    score: 100,
  },
]

const ITEMS_PER_PAGE = 3

export default function ExamList() {
  const [currentPage, setCurrentPage] = useState(1)
  const totalPages = Math.ceil(Exams.length / ITEMS_PER_PAGE)
  const paginatedExams = Exams.slice((currentPage - 1) * ITEMS_PER_PAGE, currentPage * ITEMS_PER_PAGE)
  const getStatusColor = (status: string) => {
    if (status === 'Upcoming') return 'text-green-500'
    if (status === 'Ended') return 'text-gray-500'
    return 'text-red-500'
  }
  return (
    <>
      <RootNavBar />
      <div className="min-h-screen mx-44 flex flex-col items-center justify-between mt-3 ">
        <h1 className="text-center my-4 font-bold text-3xl">My Exams</h1>
        {paginatedExams.map((exam) => (
          <Card key={exam.id} className="relative w-full mb-3 p-2 shadow-none bg-white dark:bg-[#18181b]">
            <CardHeader>
              <div className="flex w-full justify-between items-center">
                <h1 className="text-2xl font-bold w-full">
                  {exam.title}
                  <span className={`ml-2 text-sm ${getStatusColor(exam.status)}`}>{exam.description}</span>
                </h1>
                {exam.status === 'Running' && (
                  <Button color="primary" className="ml-96">
                    <Link href="/my-exams/1" style={{ textDecoration: 'none', color: 'inherit' }}>
                      Attend
                    </Link>
                  </Button>
                )}
              </div>
            </CardHeader>
            <CardBody className="px-3">
              {exam.status === 'Ended' ? (
                <div className="text-center">
                  <div className="flex justify-between">
                    <p>
                      <span className="text-[#71717a] dark:text-white"> Date : </span>
                      {new Date(exam.opensAt).toLocaleDateString('en-US', {
                        weekday: 'long',
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric',
                      })}
                    </p>
                    <p>
                      {' '}
                      <span className="text-[#71717a] dark:text-white">Score :</span> 100/100
                    </p>
                    <p>
                      {' '}
                      <span className="text-[#71717a] dark:text-white">Participants :</span> 3068
                    </p>
                  </div>
                  <div>
                    <p className="font-semibold mt-7">
                      Your result hasn&apos;t been published. You&apos;ll be notified once it&apos;s available.
                    </p>
                    <p>Congratulations! You are in the top 5%</p>
                    <p>You are on 40%</p>
                    <p className="text-red-500">You cheated!</p>
                  </div>
                </div>
              ) : (
                <div className="flex">
                  <div className="flex flex-col flex-1">
                    <p>
                      <span className="text-[#71717a] dark:text-white"> Date : </span>
                      {new Date(exam.opensAt).toLocaleDateString('en-US', {
                        weekday: 'long',
                        year: 'numeric',
                        month: 'long',
                        day: 'numeric',
                      })}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">Duration :</span> {exam.durationMinutes / 60} hr
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white"> Starts at :</span>
                      {new Date(exam.opensAt).toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' })}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">Closes at :</span>
                      {new Date(exam.closesAt).toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' })}
                    </p>
                  </div>
                  <div className="flex flex-col flex-1">
                    <p>
                      <span className="text-[#71717a] dark:text-white">Problem Solving:</span> {exam.problemSolving}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">Written :</span> {exam.written}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">MCQ :</span> {exam.mcq}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">Score :</span> {exam.score}
                    </p>
                  </div>
                </div>
              )}
            </CardBody>
          </Card>
        ))}
        <div className="flex justify-between w-full items-center my-4">
          <span className="mx-4">
            Page {currentPage} of {totalPages}
          </span>
          <PaginationButtons
            currentIndex={currentPage}
            totalItems={totalPages}
            onPrevious={() => setCurrentPage(currentPage - 1)}
            onNext={() => setCurrentPage(currentPage + 1)}
          />
        </div>
      </div>
    </>
  )
}
