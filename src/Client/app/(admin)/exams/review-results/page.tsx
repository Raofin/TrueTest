'use client'

import React, { useState } from 'react'
import { Card, CardBody, Button, Divider, Textarea, Select, SelectItem } from '@heroui/react'
import useTheme from '@/app/hooks/useTheme'

interface TestCase {
  input: string
  expectedOutput: string
  receivedOutput: string
}

interface Question {
  id: number
  title: string
  description: string
  inputFormat?: string
  outputFormat?: string
  constraints?: string
  userSubmission?: string
  testCases: TestCase[]
  points: number
  maxPoints: number
  type: 'code' | 'written'
}

interface ExamSubmission {
  candidateId: string
  questions: Question[]
}

interface ExamData {
  title: string
  candidate: string
  totalScore: string
  examDuration: string
  problemSolving: string
  writtenQuestions: string
  mcq: string
  submissions: ExamSubmission[]
}

export default function App() {
  const examData: ExamData[] = [
    {
      title: 'Learnathon 2.0',
      candidate: 'Candidate-1',
      totalScore: '100/100',
      examDuration: '0h 45m 31s',
      problemSolving: '50/50',
      writtenQuestions: '20/20',
      mcq: '30/30',
      submissions: [
        {
          candidateId: 'Candidate-1',
          questions: [
            {
              id: 1,
              title: 'Question #1',
              description: 'Write a program that takes two integers as input and prints their sum.',
              inputFormat: 'Two space-separated integers, a and b.',
              outputFormat: 'Print the sum of a and b.',
              constraints: '-10⁹ ≤ a, b ≤ 10⁹',
              userSubmission: `#include <bits/stdc++.h>
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
              points: 5,
              maxPoints: 5,
              type: 'code',
            },
            {
              id: 8,
              title: 'Question #8',
              description: 'What is the difference between a deadlock and a livelock?',
              userSubmission:
                'A deadlock occurs when processes are stuck waiting for each other indefinitely, while a livelock happens when processes continuously change states but still make no progress.',
              testCases: [],
              points: -3,
              maxPoints: 5,
              type: 'written',
            },
          ],
        },
        {
          candidateId: 'Candidate-2',
          questions: [
            {
              id: 1,
              title: 'Question #1',
              description: 'Write a program that takes two integers as input and prints their sum.',
              inputFormat: 'Two space-separated integers, a and b.',
              outputFormat: 'Print the sum of a and b.',
              constraints: '-10⁹ ≤ a, b ≤ 10⁹',
              userSubmission: `
int main() {
    int x, y;
    scanf("%d %d", &x, &y);
    printf("Candidate-2");
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
              points: 4,
              maxPoints: 5,
              type: 'code',
            },
          ],
        },
      ],
    },
  ]

  const [flag, setFlag] = useState(false)
  const Mode = useTheme()
  const [selectedCandidate, setSelectedCandidate] = useState('Candidate-1')
  const currentExamData = examData[0]
  const currentCandidateSubmission = currentExamData.submissions.find((sub) => sub.candidateId === selectedCandidate)
  const currentQ = currentCandidateSubmission?.questions || currentExamData.submissions[0].questions

  const handleCandidateChange = (value: string) => {
    setSelectedCandidate(value)
  }

  const handlePrevCandidate = () => {
    const currentIndex = currentExamData.submissions.findIndex((sub) => sub.candidateId === selectedCandidate)
    if (currentIndex > 0) {
      setSelectedCandidate(currentExamData.submissions[currentIndex - 1].candidateId)
    }
  }

  const handleNextCandidate = () => {
    const currentIndex = currentExamData.submissions.findIndex((sub) => sub.candidateId === selectedCandidate)
    if (currentIndex < currentExamData.submissions.length - 1) {
      setSelectedCandidate(currentExamData.submissions[currentIndex + 1].candidateId)
    }
  }

  return (
    <div>
      <Card className="max-w-6xl mx-auto border-none h-screen">
        <CardBody>
          <div className="space-y-4">
            <h1 className="text-2xl font-bold w-full text-center">Exam: {currentExamData.title}</h1>

            <div className="flex flex-col gap-3 text-lg">
              <div className="flex w-full items-center justify-between">
                <div className="flex gap-2 items-center">
                  <span className="text-default-500">Candidate: </span>
                  <Select
                    aria-label="Select a candidate"
                    className="w-80"
                    value={selectedCandidate}
                    onChange={(e) => handleCandidateChange(e.target.value)}
                  >
                    {currentExamData.submissions.map((sub) => (
                      <SelectItem key={sub.candidateId} value={sub.candidateId}>
                        {sub.candidateId}
                      </SelectItem>
                    ))}
                  </Select>
                </div>
                <div className="flex gap-2">
                  <Button
                    color="primary"
                    size="sm"
                    isDisabled={
                      currentExamData.submissions.findIndex((sub) => sub.candidateId === selectedCandidate) <= 0
                    }
                    onPress={handlePrevCandidate}
                  >
                    Previous Candidate
                  </Button>
                  <Button
                    color="primary"
                    size="sm"
                    isDisabled={
                      currentExamData.submissions.findIndex((sub) => sub.candidateId === selectedCandidate) >=
                      currentExamData.submissions.length - 1
                    }
                    onPress={handleNextCandidate}
                  >
                    {' '}
                    Next Candidate
                  </Button>
                </div>
              </div>

              <div className="flex w-full items-center justify-between">
                <div>
                  <span className="text-default-500">Score: </span>
                  {currentExamData.totalScore}
                </div>
                <div>
                  <span className="text-default-500">Exam Duration: </span>
                  {currentExamData.examDuration}
                </div>
              </div>

              <div className="flex w-full items-center justify-between">
                <div>
                  <span className="text-default-500">Problem Solving: </span>
                  {currentExamData.problemSolving}
                </div>
                <div>
                  <span className="text-default-500">Written Questions: </span>
                  {currentExamData.writtenQuestions}
                </div>
                <div>
                  <span className="text-default-500">MCQ: </span>
                  {currentExamData.mcq}
                </div>
              </div>
            </div>
          </div>

          <Divider className="my-5" />
          <h2 className="w-full text-center ">{selectedCandidate}</h2>

          {currentQ.map((curr, idx) => (
            <div key={idx} className="space-y-4">
              <h2 className="text-lg font-semibold">{curr.title}</h2>
              <p>{curr.description}</p>
              {curr.type === 'code' && (
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
                <h3 className="font-semibold mb-2">{curr.type === 'code' ? 'User Submission' : 'User Answer'}</h3>
                <div className="p-4 rounded-lg">
                  <div
                    className={`font-mono text-sm whitespace-pre-wrap p-2 ${
                      Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'
                    }`}
                  >
                    {curr.userSubmission}
                  </div>
                </div>
              </div>

              {curr.type === 'code' && curr.testCases.length > 0 && (
                <div>
                  <h3 className="font-semibold mb-2">
                    Test Cases (Passed {curr.testCases.length}/{curr.testCases.length})
                  </h3>
                  <div className="space-y-2">
                    <div className="grid grid-cols-3 gap-4 p-3 rounded-lg">
                      <div className="text-xs text-default-500 mb-1">Input</div>
                      <div className="text-xs text-default-500 mb-1">Expected Output</div>
                      <div className="text-xs text-default-500 mb-1">Received Output</div>
                    </div>
                    {curr.testCases.map((testCase, index) => (
                      <div key={index} className="grid grid-cols-3 gap-4 p-3 rounded-lg">
                        <div className={`${Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'} p-3 font-mono text-sm`}>
                          {testCase.input}
                        </div>
                        <div className={`${Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'} p-3 font-mono text-sm`}>
                          {testCase.expectedOutput}
                        </div>
                        <div className={`${Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'} p-3 font-mono text-sm`}>
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
                    <input name="points" type="number" className="w-16" defaultValue={curr.points} />/{curr.maxPoints}
                  </div>
                  <Button size="sm" variant="flat">
                    <input name="flag" type="checkbox" checked={flag} onChange={(e) => setFlag(e.target.checked)} />{' '}
                    Flag Solution
                  </Button>
                </div>
              </div>

              {flag && <Textarea placeholder="Flag reason" />}

              <div className="w-full flex justify-center">
                {' '}
                <Button className="my-3" color="primary">
                  {' '}
                  Save
                </Button>
              </div>
            </div>
          ))}
        </CardBody>
      </Card>
    </div>
  )
}
