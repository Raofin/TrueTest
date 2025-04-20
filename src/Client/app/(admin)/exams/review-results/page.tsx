'use client'

import React, { useState, useEffect } from 'react'
import { Card, Button, Textarea, Select, SelectItem } from '@heroui/react'
import api from '@/lib/api'
import { useSearchParams } from 'next/navigation'

interface TestCase {
  input: string
  expectedOutput: string
  receivedOutput: string
}

interface Question {
  questionId: string
  questionTitle: string
  questionDescription: string
  inputFormat?: string
  outputFormat?: string
  constraints?: string
  userAnswer?: string
  testCases: TestCase[]
  pointsAwarded: number
  maxPoints: number
  questionType: 'code' | 'written' | 'mcq'
}

interface Submission {
  accountId: string
  username: string
  email: string
  result: {
    totalScore: number
    problemSolvingScore: number
    submittedAt: string
    hasReviewed: boolean
    isReviewed: boolean
  }
  questions: Question[]
}

interface ExamData {
  title: string
  submissions: Submission[]
}

const initialExamData: ExamData = {
  title: 'Learnathon 2.0',
  submissions: [
    {
      accountId: 'a3685f64-5717-4562-b2fc-2c963f66afab',
      username: 'string',
      email: 'string',
      result: {
        totalScore: 0,
        problemSolvingScore: 0,
        submittedAt: '2025-04-15T06:35:37.373Z',
        hasReviewed: true,
        isReviewed: true,
      },
      questions: [
        {
          questionId: '1',
          questionTitle: 'Question #1',
          questionDescription: 'Write a program that takes two integers as input and prints their sum.',
          inputFormat: 'Two space-separated integers, a and b.',
          outputFormat: 'Print the sum of a and b.',
          constraints: '-10⁹ ≤ a, b ≤ 10⁹',
          userAnswer: `#include <bits/stdc++.h>
using namespace std;

int main() {
int a, b;
cin >> a >> b;
cout << "Candidate-1" << '\\n';
return 0;
}`,
          testCases: [
            {
              input: '3 7',
              expectedOutput: '10',
              receivedOutput: '10',
            },
            {
              input: '99999 -99999',
              expectedOutput: '0',
              receivedOutput: '0',
            },
          ],
          pointsAwarded: 5,
          maxPoints: 5,
          questionType: 'code',
        },
        {
          questionId: '8',
          questionTitle: 'Question #8',
          questionDescription: 'What is the difference between a deadlock and a livelock?',
          userAnswer:
            'A deadlock occurs when processes are stuck waiting for each other indefinitely, while a livelock happens when processes continuously change states but still make no progress.',
          testCases: [],
          pointsAwarded: -3,
          maxPoints: 5,
          questionType: 'written',
        },
      ],
    },
  ],
}

export default function Component() {
  const [examData] = useState<ExamData>(initialExamData)
  const searchParams = useSearchParams()
  const examId = searchParams.get('examId')
  const examTitle = searchParams.get('examTitle')
  const [flaggedQuestions, setFlaggedQuestions] = useState<{
    [accountId: string]: { [questionId: string]: boolean }
  }>({})
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await api.get(`/Review/Candidates/${examId}`)
        if (response.status === 200) {
          console.log('Review candidate submission')
        }
      } catch (error) {
        console.error('Error fetching data:', error)
      }
    }
    fetchData()
  }, [examId])

  const [selectedCandidateId, setSelectedCandidateId] = useState(examData.submissions[0]?.accountId ?? '')
  const currentSubmission = examData.submissions.find((sub) => sub.accountId === selectedCandidateId)
  const currentQuestions = currentSubmission?.questions ?? []
  const handleCandidateChange = (value: string) => {
    setSelectedCandidateId(value)
  }
  const handlePrevCandidate = () => {
    const currentIndex = examData.submissions.findIndex((sub) => sub.accountId === selectedCandidateId)
    if (currentIndex > 0) {
      setSelectedCandidateId(examData.submissions[currentIndex - 1].accountId)
    }
  }

  const handleNextCandidate = () => {
    const currentIndex = examData.submissions.findIndex((sub) => sub.accountId === selectedCandidateId)
    if (currentIndex < examData.submissions.length - 1) {
      setSelectedCandidateId(examData.submissions[currentIndex + 1].accountId)
    }
  }

  const handleFlagChange = (accountId: string, questionId: string) => {
    setFlaggedQuestions((prevFlags) => {
      const newFlags = { ...prevFlags }
      newFlags[accountId] ??= {}
      newFlags[accountId][questionId] = !newFlags[accountId][questionId]
      return newFlags
    })
  }

  const isQuestionFlagged = (accountId: string, questionId: string) => {
    return flaggedQuestions[accountId]?.[questionId] ?? false
  }

  return (
    <div className="min-h-screen flex flex-col justify-between">
      <h2 className="text-2xl font-bold text-center my-5">Review Results</h2>
      <div className="w-full px-12 border-none flex flex-col gap-4 ">
        <Card className="space-y-4 p-5 bg-white dark:bg-[#18181b] shadow-none">
          <h1 className="text-xl font-semibold w-full text-center">Exam: {examTitle}</h1>

          <div className="flex flex-col gap-3 ">
            <div className="flex w-full items-center justify-between">
              <div className="flex gap-2 items-center">
                <span className="text-default-500">Candidate: </span>
                <Select
                  aria-label="Select a candidate"
                  className="w-80"
                  value={selectedCandidateId}
                  onChange={(e: { target: { value: string } }) => handleCandidateChange(e.target.value)}
                >
                  {examData.submissions.map((sub) => (
                    <SelectItem key={sub.accountId}>
                      {sub.accountId} ({sub.username ?? sub.email})
                    </SelectItem>
                  ))}
                </Select>
              </div>
              <div className="flex gap-2">
                <Button
                  size="sm"
                  isDisabled={examData.submissions.findIndex((sub) => sub.accountId === selectedCandidateId) <= 0}
                  onPress={handlePrevCandidate} >
                  Previous
                </Button>
                <Button
                  size="sm"
                  isDisabled={
                    examData.submissions.findIndex((sub) => sub.accountId === selectedCandidateId) >=
                    examData.submissions.length - 1
                  }
                  onPress={handleNextCandidate}
                >
                  Next
                </Button>
              </div>
            </div>

            {currentSubmission && (
              <div className="flex items-center justify-between">
                <div>
                  <span className="text-default-500">Score: </span>
                  {currentSubmission.result.totalScore}/
                  {examData.submissions.reduce(
                    (sum, sub) => sum + sub.questions.reduce((qSum, q) => qSum + q.maxPoints, 0),
                    0
                  )}
                </div>
                <div>
                  <span className="text-default-500">Submitted At: </span>
                  {new Date(currentSubmission.result.submittedAt).toLocaleString()}
                </div>
              </div>
            )}
          </div>
        </Card>
        <div className="bg-white dark:bg-[#18181b] p-5 rounded-xl">
          <h2 className="w-full text-center ">
            {currentSubmission?.username ?? currentSubmission?.email ?? 'No Candidate Selected'}
          </h2>
          {currentQuestions.map((curr) => (
            <div key={curr.questionId} className="space-y-4">
              <h2 className="text-lg font-semibold">{curr.questionTitle}</h2>
              <p>{curr.questionDescription}</p>
              {curr.questionType === 'code' && (
                <>
                  {curr.inputFormat && (
                    <div>
                      <h3 className="font-semibold mb-1">Input Format:</h3>
                      <p>{curr.inputFormat}</p>
                    </div>
                  )}
                  {curr.outputFormat && (
                    <div>
                      <h3 className="font-semibold mb-1">Output Format:</h3>
                      <p>{curr.outputFormat}</p>
                    </div>
                  )}
                  {curr.constraints && (
                    <div>
                      <h3 className="font-semibold mb-1">Constraints:</h3>
                      <p>{curr.constraints}</p>
                    </div>
                  )}
                </>
              )}
              <div>
                <h3 className="font-semibold mb-2">
                  {curr.questionType === 'code' ? 'User Submission' : 'User Answer'}
                </h3>
                <Card className={`p-4 rounded-lg bg-[#eeeef0] dark:bg-[#27272a] shadow-none`}>
                  <div className={`font-mono text-sm whitespace-pre-wrap p-2`}>{curr.userAnswer}</div>
                </Card>
              </div>

              {curr.questionType === 'code' && curr.testCases.length > 0 && (
                <div>
                  <h3 className="font-semibold mb-2">
                    Test Cases (Passed {curr.testCases.filter((tc) => tc.expectedOutput === tc.receivedOutput).length}/
                    {curr.testCases.length})
                  </h3>
                  <div className="space-y-2">
                    <div className="grid grid-cols-3 gap-4 p-3 rounded-lg">
                      <div className="text-xs text-default-500 mb-1">Input</div>
                      <div className="text-xs text-default-500 mb-1">Expected Output</div>
                      <div className="text-xs text-default-500 mb-1">Received Output</div>
                    </div>
                    {curr.testCases.map((testCase) => (
                      <div key={testCase.input} className="grid grid-cols-3 gap-4 p-3 ">
                        <div className={`p-3 font-mono text-sm h-20 rounded-lg bg-[#eeeef0] dark:bg-[#27272a]`}>
                          {testCase.input}
                        </div>
                        <div className={`p-3 font-mono text-sm h-20 rounded-lg bg-[#eeeef0] dark:bg-[#27272a]`}>
                          {testCase.expectedOutput}
                        </div>
                        <div
                          className={`p-3 font-mono text-sm h-20 rounded-lg ${
                            testCase.expectedOutput === testCase.receivedOutput
                              ? 'bg-green-100 dark:bg-green-800 text-green-700 dark:text-green-300'
                              : 'bg-red-100 dark:bg-red-800 text-red-700 dark:text-red-300'
                          } font-mono text-sm h-20 rounded-lg`}
                        >
                          {testCase.receivedOutput}
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              )}

              <div>
                <h3 className="font-semibold mb-2">Result</h3>
                <div className="flex items-center gap-5">
                  <div className="flex items-center gap-3">
                    <span>Points</span>
                    <input name="points" type="number" className="w-16" defaultValue={curr.pointsAwarded} />/
                    {curr.maxPoints}
                  </div>
                  <Button size="sm" variant="flat">
                    <input
                      name="flag"
                      type="checkbox"
                      checked={isQuestionFlagged(selectedCandidateId, curr.questionId)}
                      onChange={() => handleFlagChange(selectedCandidateId, curr.questionId)}
                    />
                    Flag Solution
                  </Button>
                </div>
              </div>
              {isQuestionFlagged(selectedCandidateId, curr.questionId) && (
                <Textarea className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl" placeholder="Flag reason" />
              )}

              <div className="w-full flex justify-center">
                <Button className="my-3" color="primary">
                  Save
                </Button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
