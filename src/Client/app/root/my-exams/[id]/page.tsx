'use client'
import React, { useEffect, useState } from 'react'
import { Card, CardBody, Button, Checkbox, Pagination } from '@heroui/react'
import CodeEditor from 'app/root/my-exams/code-editor'
import 'app/globals.css'
interface Question {
  id: number
  title: string
  description: string
  questionTypeId: number // 1=mcq , 2=written, 3=coding
  options?: string[]
  points: number
}

export default function Component() {
  const [currentPage, setCurrentPage] = React.useState(1)
  const [timeLeft, setTimeLeft] = React.useState(3600)
  const [agreedToTerms, setAgreedToTerms] = React.useState({
    capture: false,
    screenMonitor: false,
    audio: false,
    clipboard: false,
  })
  const [examStarted, setExamStarted] = React.useState(false)
  const [answers, setAnswers] = React.useState<{ [key: number]: string }>({})
  const [Mode,setMode]=useState<string|null>(null);
  useEffect(() => {
      if (typeof window !== 'undefined') {
        setMode(localStorage.getItem("theme"));
      }
    }, []);
  const examData = {
    title: 'Star Coder 2025',
    totalQuestions: 30,
    duration: '1:00:00',
    problemSolving: 10,
    written: 10,
    mcq: 30,
    totalScore: 100,
  }

  const questions: Question[] = [
    ...[...Array(7)].map((_, index) => ({
      id: index + 1,
      title: `Question : ${index + 1}`,
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
    ...[...Array(3)].map((_, index) => ({
      id: index + 11,
      title: `Question #${index + 1}`,
      description: 'Write a function to implement the following logic...',
      questionTypeId: 3,
      points: 10,
    })),
  ]

  const getQuestionsForCurrentPage = () => {
    const regularQuestions = questions.filter((q) => q.questionTypeId !== 3)
    const codingQuestions = questions.filter((q) => q.questionTypeId === 3)

    const regularQuestionsPages = Math.ceil(regularQuestions.length / 5)
    if (currentPage <= regularQuestionsPages) {
      const startIndex = (currentPage - 1) * 5
      return regularQuestions.slice(startIndex, startIndex + 5)
    }
    const codingIndex = currentPage - regularQuestionsPages - 1
    if (codingIndex >= 0 && codingIndex < codingQuestions.length) {
      return [codingQuestions[codingIndex]]
    }

    return []
  }

  const getTotalPages = () => {
    const regularQuestions = questions.filter((q) => q.questionTypeId !== 3).length
    const codingQuestions = questions.filter((q) => q.questionTypeId === 3).length
    return Math.ceil(regularQuestions / 5) + codingQuestions
  }

  React.useEffect(() => {
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

  const formatTime = (seconds: number) => {
    const hours = Math.floor(seconds / 3600)
    const minutes = Math.floor((seconds % 3600) / 60)
    const secs = seconds % 60
    return `${hours}:${minutes.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`
  }

  if (!examStarted) {
    return (
      <div className="pt-8">
        <Card className="max-w-3xl mx-auto border-none">
          <CardBody>
            <div className="space-y-3">
              <div>
                <h1 className="text-2xl font-bold">{examData.title}</h1>
                <span className="text-sm text-default-500">Running</span>
              </div>

              <div className="grid grid-cols-2 gap-2 text-sm">
                <div>Date: Friday, 21 Nov 2026</div>
                <div className="text-right">Problem Solving: {examData.problemSolving}</div>
                <div>Starts at: 9:00 PM</div>
                <div className="text-right">Written: {examData.written}</div>
                <div>Closes at: 10:20 PM</div>
                <div className="text-right">MCQ: {examData.mcq}</div>
                <div>Duration: {examData.duration}</div>
                <div className="text-right">Score: {examData.totalScore}</div>
              </div>

              <hr className="my-2" />

              <div className="space-y-2">
                <h2 className="text-lg font-semibold">Instructions</h2>
                <ol className="list-decimal list-inside space-y-2 text-sm">
                  <li>Please ensure your webcam and microphone are enabled throughout the exam.</li>
                  <li>There should be no one else visible in the webcam area during the exam.</li>
                  <li>
                    A stable internet connection is required. If you are disconnected for more than 2 minutes, your
                    answers will be automatically submitted.
                  </li>
                  <li>
                    Your face must be clearly visible in the webcam at all times, and your eyes should remain on the
                    screen. Please do not look away from the screen.
                  </li>
                  <li>The exam will automatically switch you to full-screen mode. Please do not try to change it.</li>
                  <li>Do not attempt to copy or paste anything during the exam. It will be detected and flagged.</li>
                </ol>
              </div>

              <div className="space-y-1">
                <h3 className="font-semibold">
                  Please read and agree to the following terms to proceed with the exam:
                </h3>
                <div className="space-y-2 text-sm">
                  <Checkbox
                    isSelected={agreedToTerms.capture}
                    onValueChange={(value) => setAgreedToTerms((prev) => ({ ...prev, capture: value }))}
                  >
                    I agree that images will be captured of me every 5 seconds.
                  </Checkbox>
                  <Checkbox
                    isSelected={agreedToTerms.screenMonitor}
                    onValueChange={(value) => setAgreedToTerms((prev) => ({ ...prev, screenMonitor: value }))}
                  >
                    I agree that my screen will be monitored during the exam.
                  </Checkbox>
                  <Checkbox
                    isSelected={agreedToTerms.audio}
                    onValueChange={(value) => setAgreedToTerms((prev) => ({ ...prev, audio: value }))}
                  >
                    I agree that my audio will be recorded throughout the exam.
                  </Checkbox>
                  <Checkbox
                    isSelected={agreedToTerms.clipboard}
                    onValueChange={(value) => setAgreedToTerms((prev) => ({ ...prev, clipboard: value }))}
                  >
                    I agree that my clipboard activity will be tracked.
                  </Checkbox>
                </div>
              </div>

              <Button
                color="primary"
                className="w-full"
                isDisabled={!Object.values(agreedToTerms).every(Boolean)}
                onPress={() => setExamStarted(true)}
              >
                Start Exam
              </Button>
            </div>
          </CardBody>
        </Card>
      </div>
    )
  }

  const currentQuestions = getQuestionsForCurrentPage()

  if (currentQuestions.length === 0) {
    return (
      <div className="text-white">
        <div className="flex justify-center items-center h-full">
          <p className="mt-16">Loading questions...</p>
        </div>
      </div>
    )
  }

  return (
    <div className="">
      {examStarted && (
        <div className="py-3 border-b border-white/10  z-50 shadow">
          <div className="max-w-7xl mx-auto flex items-center justify-between">
            <div className="flex items-center gap-2">
              <span className="font-bold ">{examData.title}</span>
            </div>
            <div className="flex items-center gap-4 ">
              <div>
                Questions Left: {questions.length - currentPage}/{questions.length}
              </div>
              <div className={`font-mono ${timeLeft < 300 ? 'text-danger' : ''}`}>
                Time Left: {formatTime(timeLeft)}
              </div>
              <Button color="primary" size="sm">
                Submit Exam
              </Button>
            </div>
          </div>
        </div>
      )}

      <div>
        <div className="mx-5 mt-3  border-none px-8">
          <div className={`space-y-8 rounded-lg p-5 `}>
            {currentQuestions.map((question) => (
              <div key={question.id} className="space-y-4">
                <div className="w-full flex justify-between">
                  <h2 className="text-lg font-semibold">{question.title}</h2>
                  <p>points: {question.points}</p>
                </div>

                {question.questionTypeId === 1 && question.options && (
                  <div className={`space-y-4 p-4 rounded-lg ${Mode === 'dark' ? 'bg-[#18181b]' : 'bg-white'}`}>
                    <p>{question.description}</p>
                    {question.options.map((option, index) => (
                      <div key={index} className="flex items-center gap-2 p-3 rounded-lg hover:bg-white/5">
                        <Checkbox value={option}>{option}</Checkbox>
                      </div>
                    ))}
                  </div>
                )}

                {question.questionTypeId === 2 && (
                  <div className={`space-y-4 p-4 rounded-lg ${Mode === 'dark' ? 'bg-[#18181b]' : 'bg-white'}`}>
                    <p>{question.description}</p>
                    <textarea
                      className={`w-full border border-white/10 rounded-lg p-3 ${
                        Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'
                      }`}
                      placeholder="Type your answer here..."
                      value={answers[question.id] || ''}
                      onChange={(e) =>
                        setAnswers({
                          ...answers,
                          [question.id]: e.target.value,
                        })
                      }
                    />
                  </div>
                )}

                {question.questionTypeId === 3 && <CodeEditor />}
              </div>
            ))}
          </div>

          <div className="flex justify-center items-end py-6">
            <Pagination
              total={getTotalPages()}
              page={currentPage}
              onChange={setCurrentPage}
              color="primary"
              showControls
              className="gap-2"
            />
          </div>
        </div>
      </div>
    </div>
  )
}
