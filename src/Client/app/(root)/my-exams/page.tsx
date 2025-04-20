'use client'

import React, { useEffect, useState } from 'react'
import { Button, Card, CardBody, CardHeader, Link } from '@heroui/react'
import PaginationButtons from '@/components/ui/pagination-button'
import RootNavBar from '@/app/(root)/root-navbar'
import api from '@/lib/api'
import toast from 'react-hot-toast'
import { useRouter } from 'next/navigation'
import { convertUtcToLocalTime, FormatTimeHourMinutes } from '@/components/format-date-time'

interface Exam {
  examId: number
  title: string
  description: string
  totalPoints:number
  durationMinutes: number
  opensAt: string
  closesAt: string
  status: string
  problemSolvingPoints: number
  writtenPoints: number
  mcqPoints: number
}

const ITEMS_PER_PAGE = 3
export default function ExamList() {
  const route = useRouter()
  useEffect(() => {
    const FetchExamData = async () => {
      try {
        const res = await api.get('/Candidate/Exams')
        if (res.status === 200) setExams(res.data)
        else if (res.status === 401) {
          toast.error('Unauthorized')
          route.push('/signin')
        } else if (res.status === 500) toast.error('Internal Server Error')
      } catch {}
    }
    FetchExamData()
  }, [route])
  
  const [currentPage, setCurrentPage] = useState(1)
  const [exams, setExams] = useState<Exam[]>([])
  const totalPages = Math.ceil(exams.length / ITEMS_PER_PAGE)
  const paginatedExams = exams.slice((currentPage - 1) * ITEMS_PER_PAGE, currentPage * ITEMS_PER_PAGE)
  const getStatusColor = (status: string) => {
    if (status === 'Scheduled') return 'text-green-500'
    if (status === 'Ended') return 'text-gray-500'
    return 'text-red-500'
  }
  return (
    <div >
      <RootNavBar />
      <div className="min-h-screen mx-44 flex flex-col items-center justify-between mt-3 ">
       <div className='w-full'>
       <h1 className="text-center my-4 font-bold text-3xl">My Exams</h1>
        {paginatedExams ? paginatedExams.map((exam) => (
          <Card key={exam.examId} className="relative w-full mb-3 p-2 shadow-none bg-white dark:bg-[#18181b]">
            <CardHeader>
              <div className="flex w-full justify-between items-center">
                <h1 className="text-2xl font-bold w-full">
                  {exam.title}
                  <span className={`ml-2 text-sm ${getStatusColor(exam.status)}`}>{exam.status}</span>
                </h1>
                {exam.status === 'Running' && (
                  <Button color="primary" className="ml-96">
                    <Link href={`/my-exams/${exam.examId}`} style={{ textDecoration: 'none', color: 'inherit' }}>
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
                      <span className="text-[#71717a] dark:text-white">Start Time : </span>{convertUtcToLocalTime(exam.opensAt)}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">End Time : </span>{convertUtcToLocalTime(exam.closesAt)}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">Score : </span>{exam.totalPoints}
                    </p>
                  </div>
                  <div>
                    <p className="font-semibold mt-7">
                      Your result hasn&apos;t been published. You&apos;ll be notified once it&apos;s available.
                    </p>
                    {/* <p>Congratulations! You are in the top 5%</p> */}
                    {/* <p>You are on 40%</p> */}
                    {/* <p className="text-red-500">You cheated!</p> */}
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
                      <span className="text-[#71717a] dark:text-white">Duration :</span> <FormatTimeHourMinutes minute={exam.durationMinutes}/> hr
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white"> Starts at :</span>
                      {convertUtcToLocalTime(exam.opensAt)}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">Closes at :</span>
                      {convertUtcToLocalTime(exam.closesAt)}
                    </p>
                  </div>
                  <div className="flex flex-col flex-1">
                    <p>
                      <span className="text-[#71717a] dark:text-white">Problem Solving:</span>{' '}
                      {exam.problemSolvingPoints}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">Written :</span> {exam.writtenPoints}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">MCQ :</span> {exam.mcqPoints}
                    </p>
                    <p>
                      <span className="text-[#71717a] dark:text-white">Score : {exam.totalPoints}</span>
                    </p>
                  </div>
                </div>
              )}
            </CardBody>
          </Card>
        )):"No exam found"}
       </div>
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
    </div>
  )
}
