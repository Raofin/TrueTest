'use client'

import { Card, CardBody, Checkbox, Button } from '@heroui/react'
import { useState } from 'react'
import RootNavBar from '@/app/(root)/root-navbar'

interface ExamData {
  title: string
  totalQuestions: number
  duration: string
  problemSolving: number
  written: number
  mcq: number
  totalScore: number
}
interface PageProps {
  readonly examData: ExamData
  readonly startExam: () => void
  readonly setExamStarted: (started: boolean) => void
}

export default function StartExam({ examData, setExamStarted, startExam }: PageProps) {
  const [agreedToTerms, setAgreedToTerms] = useState({
    capture: false,
    screenMonitor: false,
    audio: false,
    clipboard: false,
  })

  return (
    <div>
      <RootNavBar />
      <div className="min-h-screen flex flex-col items-center justify-center">
        <Card className=" max-w-3xl mx-auto p-3 border-none shadow-none bg-white dark:bg-[#18181b]">
          <CardBody>
            <div className="space-y-3 ">
              <div className="flex w-full justify-between">
                <h1 className="text-2xl font-bold w-full">
                  {examData.title}
                  <span className={`ml-2 text-sm text-red-500`}>Running</span>
                </h1>
                <div className="flex items-center gap-1 before:content-[''] before:w-2 before:h-2 before:bg-red-500 before:rounded-full">
                  <p>45:03s</p>
                  <p>left</p>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-2 text-sm">
                <div>
                  <span className="text-[#71717a] dark:text-white">Date:</span> Friday, 21 Nov 2026
                </div>
                <div className="text-right">
                  <span className="text-[#71717a] dark:text-white">Problem Solving:</span> {examData.problemSolving}
                </div>
                <div>
                  <span className="text-[#71717a] dark:text-white">Starts at:</span> 9:00 PM
                </div>
                <div className="text-right">
                  <span className="text-[#71717a] dark:text-white">Written:</span> {examData.written}
                </div>
                <div>
                  <span className="text-[#71717a] dark:text-white">Closes at:</span> 10:20 PM
                </div>
                <div className="text-right">
                  <span className="text-[#71717a] dark:text-white">MCQ:</span> {examData.mcq}
                </div>
                <div>
                  <span className="text-[#71717a] dark:text-white">Duration:</span> {examData.duration}
                </div>
                <div className="text-right">
                  <span className="text-[#71717a] dark:text-white">Score: </span>
                  {examData.totalScore}
                </div>
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
              <div className="w-full text-center">
                <Button
                  color="primary"
                  isDisabled={!Object.values(agreedToTerms).every(Boolean)}
                  onPress={() => {
                    setExamStarted(true)
                    startExam()
                  }}
                >
                  Start Exam
                </Button>
              </div>
            </div>
          </CardBody>
        </Card>
      </div>
    </div>
  )
}
