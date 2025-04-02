'use client'

import React, { useEffect, useMemo, useState } from 'react'
import { Button, Pagination } from '@heroui/react'
import CodeEditor from '@/app/components/submission/code-editor'
import '@/app/globals.css'
import getQuestionsForCurrentPage from '@/app/components/currpage-ques'
import formatTime from '@/app/components/ui/format-time'
import StartExam from '@/app/components/start-exam'
import WrittenSubmission from '@/app/components/submission/written-submit'
import MCQSubmission from '@/app/components/submission/mcq-submit'
import Link from 'next/link'
import Logo from '@/app/components/ui/logo/page'

interface Question {
  id: number
  title: string
  description: string
  questionTypeId: number // 1=mcq , 2=written, 3=coding
  options?: string[]
  points: number
}

export default function Component() {
  const [currentPage, setCurrentPage] = useState(1)
  const [timeLeft, setTimeLeft] = useState(3600)
  const [examStarted, setExamStarted] = useState(false)
  const [totalPage, setTotalPage] = useState(1);
  const [answers, setAnswers] = useState<{ [key: number]: string | string[] }>({})
  const [regularQues, setRegularQues] = useState(0)
  const [codingQues, setCodingQues] = useState(0)
  const [isFullscreen, setIsFullscreen] = useState(false);
  const [questionsLeft, setQuestionsLeft] = useState(0);
  
  const examData = {
    id:1,
    title: 'Star Coder 2025',
    totalQuestions: 30,
    duration: '1:00:00',
    problemSolving: 10,
    written: 10,
    mcq: 30,
    totalScore: 100,
  }

  const questions: Question[] = useMemo(
    () => [
      ...Array.from({ length: 7 }, (_, index) => ({
        id: index + 1,
        title: `Question #${index + 1}`,
        description:
          index % 2 === 0
            ? 'What is the difference between var, let, and const in JavaScript?'
            : 'Which of the following is true about React hooks?',
        questionTypeId: index % 2 === 0 ? 2 : 1,
        options:
          index % 2 === 0
            ? undefined
            : [
                'They can only be used in class components',
                'They must be called at the top level of a component',
                'They can be called conditionally',
                'They can only be used in functional components',
              ],
        points: 3,
      })),
      ...Array.from({ length: 3 }, (_, index) => ({
        id: index + 8,
        title: `Question #${index + 1}`,
        description: 'Write a function to implement the following logic...',
        questionTypeId: 3,
        points: 10,
      })),
    ],
    []
  )

  useEffect(() => {
    const regularQuestionsCount = questions.filter((q) => q.questionTypeId !== 3).length
    const codingQuestionsCount = questions.filter((q) => q.questionTypeId === 3).length
    setRegularQues(regularQuestionsCount)
    setCodingQues(codingQuestionsCount)
    setTotalPage(Math.ceil(regularQuestionsCount / 5) + codingQuestionsCount)
    setQuestionsLeft(regularQuestionsCount + codingQuestionsCount)
  }, [questions])

  useEffect(() => {
    if (examStarted) {
      const timer = setInterval(() => {
        setTimeLeft((prev) => {
          if (prev <= 1) {
            clearInterval(timer)
            return 0
          }
          return prev - 1
        })
      }, 1000)

      return () => clearInterval(timer)
    }
  }, [examStarted])

  useEffect(() => {
    let count = 0
    questions.forEach((question) => {
      if (answers[question.id]) {
        if (Array.isArray(answers[question.id])) {
          if (answers[question.id].length > 0) {
            count++
          }
        } else if (typeof answers[question.id] === 'string') {
          if ((answers[question.id] as string).trim()) {
            count++
          }
        }
      }
    })
    setQuestionsLeft(regularQues + codingQues - count)
  }, [answers, questions, regularQues, codingQues])

  useEffect(() => {
    const handleFullscreenChange = () => {
      if (!document.fullscreenElement && !isFullscreen) {
        alert('You cannot exit fullscreen during the exam!')
        document.documentElement.requestFullscreen().catch((err) => {
          console.error('Error re-entering fullscreen:', err)
        })
      }
    }
    document.addEventListener('fullscreenchange', handleFullscreenChange)
    return () => {
      document.removeEventListener('fullscreenchange', handleFullscreenChange)
    }
  }, [isFullscreen])

  const exitFullscreen = () => {
    if (document.exitFullscreen) {
      setIsFullscreen(true)
      document.exitFullscreen().catch((err) => {
        console.error('Error exiting fullscreen:', err)
      })
    }
  }
  const startExam = () => {
    setExamStarted(true)
    if (document.documentElement.requestFullscreen) {
      document.documentElement.requestFullscreen().catch((err) => {
        console.error('Error attempting fullscreen:', err)
      })
    }
  }

  if (!examStarted) {
    return <StartExam examData={examData} setExamStarted={setExamStarted} startExam={startExam} />
  }
  const currentQuestions = getQuestionsForCurrentPage({ currentPage, questions, perpageques: 5 })

  if (currentQuestions.length === 0) {
    return (
      <div className="flex justify-center items-center h-full">
        <p className="mt-16">Loading questions...</p>
      </div>
    )
  }

  return (
    <>
      {examStarted && (
        <div className="py-3 mx-3">
          <div className="w-full px-2 flex items-center justify-between">
            <div className="flex items-center gap-2">
              <Logo/>
            </div>
            <div className="flex gap-4 rounded-full border-small border-default-200/20 bg-background/60 px-4 py-2 shadow-medium backdrop-blur-md backdrop-saturate-150 dark:bg-default-100/50">
              <div>
                Questions Left: {questionsLeft}/{codingQues + regularQues}
              </div>
              <div className="flex items-center gap-1 before:content-[''] before:w-2 before:h-2 before:bg-red-500 before:rounded-full">
                <p>Time Left : </p>
                <p className={`font-mono ml-1 ${timeLeft < 300 ? 'text-danger' : 'text-success'}`}>
                  {formatTime(timeLeft)}s
                </p>
              </div>
            </div>
            <Link href="/my-exams">
              <Button onPress={exitFullscreen} color="primary" size="md" radius='full'>
                Submit Exam
              </Button>
            </Link>
          </div>
        </div>
      )}
      <div className="mx-5 mt-3  border-none px-8">
        <div className={`space-y-8 rounded-lg `}>
          {currentQuestions.map((question) => (
            <div key={question.id} className="space-y-4">
              {question.questionTypeId === 1 && question.options && (
                <MCQSubmission
                  question={question}
                  answers={answers}
                  setAnswers={setAnswers}
                  options={question.options}
                />
              )}
              {question.questionTypeId === 2 && (
                <WrittenSubmission question={question} setAnswers={setAnswers} answers={answers} />
              )}
              {question.questionTypeId === 3 && (
                <>
                  <div className="w-full flex justify-between">
                    <h2 className="text-lg font-semibold">{question.title}</h2>
                    <p>points: {question.points}</p>
                  </div>
                  <CodeEditor questionId={question.id} setAnswers={setAnswers} />
                </>
              )}
            </div>
          ))}
        </div>
        <div className="flex justify-center items-end py-6">
          <Pagination
            total={totalPage}
            page={currentPage}
            onChange={setCurrentPage}
            color="primary"
            showControls
            className="gap-2"
          />
        </div>
      </div>
    </>
  )
}
