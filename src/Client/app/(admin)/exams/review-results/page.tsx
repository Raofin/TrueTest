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
  totalPoints:number
}

interface CandidateData {
  account: {
    accountId: string
    username: string
    email: string
  }
  result: {
    totalScore: number
    problemSolvingScore: number
    writtenScore: number
    mcqScore: number
    startedAt: string
    submittedAt: string
    hasCheated: boolean
    isReviewed: boolean
  }
}

interface ProblemSubmission {
  questionId: string
  problemSubmissionId: string
  code: string
  language: string
  attempts: number
  score: number
  isFlagged: boolean
  flagReason: string | null
  testCaseOutputs: TestCase[]
}

interface WrittenSubmission {
  questionId: string
  writtenSubmissionId: string
  answer: string
  score: number
}

interface McqSubmission {
  questionId: string
  mcqSubmissionId: string
  answerOptions: string
  score: number
}

interface CandidateSubmission {
  problem: ProblemSubmission[]
  written: WrittenSubmission[]
  mcq: McqSubmission[]
}

export default function Component() {
  const [examData, setExamData] = useState<ExamData>()
  const [candidateList, setCandidateList] = useState<CandidateData[]>([])
  const [selectedCandidateSubmission, setSelectedCandidateSubmission] = useState<CandidateSubmission | null>(null)
  const searchParams = useSearchParams()
  const examId = searchParams.get('examId')
  const [selectedCandidateId, setSelectedCandidateId] = useState<string>('')
  const [flaggedQuestions, setFlaggedQuestions] = useState<{
    [accountId: string]: { [questionId: string]: boolean }
  }>({})

  useEffect(() => {
    const fetchCandidateData = async () => {
      try {
        const response = await api.get(`/Review/Candidates/${examId}`);
        if (response.status === 200) {
          setExamData(response.data.exam);
          setCandidateList(response.data.candidates);
          if (response.data.candidates.length > 0) {
            setSelectedCandidateId(response.data.candidates[0].account.accountId);
          }
        }
      } catch (error) {
        console.error('Error fetching candidate data:', error);
      }
    };
    if (examId) fetchCandidateData();
  }, [examId]);
  
  useEffect(() => {
    const fetchSubmissionData = async () => {
      try {
        if (!selectedCandidateId || !examId) return;
        const result = await api.get(`/Review/Candidates/${examId}/${selectedCandidateId}`);
        if (result.status === 200) {
          if (result.data) {
            setSelectedCandidateSubmission(result.data.submission);
          } else {
            console.warn('missing submission data');
            setSelectedCandidateSubmission({
              problem: [],
              written: [],
              mcq: []
            });
          }
        }
      } catch (error) {
        console.error('Error fetching submission data:', error);
        setSelectedCandidateSubmission(null);
      }
    };
  
    fetchSubmissionData();
  }, [examId, selectedCandidateId]);
  const handleCandidateChange = (value: string) => {
    setSelectedCandidateId(value)
  }
  const handlePrevCandidate = () => {
    const currentIndex = candidateList.findIndex((candidate) => candidate.account.accountId === selectedCandidateId)
    if (currentIndex > 0) {
      setSelectedCandidateId(candidateList[currentIndex - 1].account.accountId)
    }
  }
  const handleNextCandidate = () => {
    const currentIndex = candidateList.findIndex((candidate) => candidate.account.accountId === selectedCandidateId)
    if (currentIndex < candidateList.length - 1) {
      setSelectedCandidateId(candidateList[currentIndex + 1].account.accountId)
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
  const selectedCandidate = candidateList.find((candidate) => candidate.account.accountId === selectedCandidateId)
  return (
    <div className="mx-44 flex flex-col justify-between">
      <h2 className="text-2xl font-bold text-center my-5">Review Results</h2>
      <div className="w-full px-12 border-none flex flex-col gap-4">
        <Card className="space-y-4 p-5 bg-white dark:bg-[#18181b] shadow-none border-none">
          <h1 className="text-xl font-semibold w-full text-center">Exam: {examData?.title}</h1>
          <div className="flex flex-col gap-3">
            <div className="flex w-full items-center justify-between">
              <div className="flex gap-2 items-center">
                <span className="text-default-500">Candidate: </span>
                <Select
                  aria-label="Select a candidate"
                  className="w-80"
                  value={selectedCandidateId}
                  onChange={(e: { target: { value: string } }) => handleCandidateChange(e.target.value)}
                >
                  {candidateList?.map((candidate) => (
                    <SelectItem key={candidate.account.accountId}>
                      {candidate.account.username ?? candidate.account.email}
                    </SelectItem>
                  ))}
                </Select>
              </div>
              <div className="flex gap-2">
                <Button
                  size="sm"
                  isDisabled={candidateList.findIndex((candidate) => candidate.account.accountId === selectedCandidateId) <= 0}
                  onPress={handlePrevCandidate}>
                  Previous
                </Button>
                <Button
                  size="sm"
                  isDisabled={
                    candidateList.findIndex((candidate) => candidate.account.accountId === selectedCandidateId) >=
                    candidateList.length - 1}
                  onPress={handleNextCandidate}>
                  Next
                </Button>
              </div>
            </div>
            {selectedCandidate && (
              <div className="flex items-center justify-between">
                <div>
                  <span className="text-default-500">Score: </span>
                   {selectedCandidate.result.totalScore}/{examData?.totalPoints} 
                </div>
                <div>
                  <span className="text-default-500">Submitted At: </span>
                   {new Date(selectedCandidate.result.submittedAt).toLocaleString()}
                </div>
              </div>
            )}
          </div>
        </Card>
        {selectedCandidateId && selectedCandidateSubmission && (
          <div className="bg-white dark:bg-[#18181b] p-5 rounded-xl">
            <h2 className="w-full text-center">
              {selectedCandidate?.account.username ?? selectedCandidate?.account.email ?? 'No Candidate Selected'}
            </h2>
            {selectedCandidateSubmission.problem.length > 0 && (
              <div className="mb-8">
                <h2 className="text-lg font-semibold mb-4">Problem Solving Submissions</h2>
                {selectedCandidateSubmission.problem.map((submission) => (
                  <div key={submission.questionId} className="space-y-4 mb-6 p-4 border rounded-lg">
                    <h3 className="font-medium">Question ID: {submission.questionId}</h3>
                    <div>
                      <h4 className="font-semibold mb-2">Code Submission:</h4>
                      <Card className="p-4 rounded-lg bg-[#eeeef0] dark:bg-[#27272a] shadow-none">
                        <div className="font-mono text-sm whitespace-pre-wrap p-2">{submission.code}</div>
                      </Card>
                    </div>
                    <div>
                      <h4 className="font-semibold mb-2">Result</h4>
                      <div className="flex items-center gap-5">
                        <div className="flex items-center gap-3">
                          <span>Points</span>
                          <input name="points" type="number" className="w-16" defaultValue={submission.score} />
                        </div>
                        <Button size="sm" variant="flat">
                          <input
                            name="flag"
                            type="checkbox"
                            checked={isQuestionFlagged(selectedCandidateId, submission.questionId)}
                            onChange={() => handleFlagChange(selectedCandidateId, submission.questionId)}
                          />
                          Flag Solution
                        </Button>
                      </div>
                    </div>
                    {isQuestionFlagged(selectedCandidateId, submission.questionId) && (
                      <Textarea className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl" placeholder="Flag reason" />
                    )}
                  </div>
                ))}
              </div>
            )}
            {selectedCandidateSubmission.written.length > 0 && (
              <div className="mb-8">
                <h2 className="text-lg font-semibold mb-4">Written Submissions</h2>
                {selectedCandidateSubmission.written.map((submission) => (
                  <div key={submission.questionId} className="space-y-4 mb-6 p-4 border rounded-lg">
                    <h3 className="font-medium">Question ID: {submission.questionId}</h3>
                    <div>
                      <h4 className="font-semibold mb-2">Answer:</h4>
                      <Card className="p-4 rounded-lg bg-[#eeeef0] dark:bg-[#27272a] shadow-none">
                        <div className="whitespace-pre-wrap p-2">{submission.answer}</div>
                      </Card>
                    </div>
                    <div>
                      <h4 className="font-semibold mb-2">Result</h4>
                      <div className="flex items-center gap-5">
                        <div className="flex items-center gap-3">
                          <span>Points</span>
                          <input name="points" type="number" className="w-16" defaultValue={submission.score} />
                        </div>
                        <Button size="sm" variant="flat">
                          <input
                            name="flag"
                            type="checkbox"
                            checked={isQuestionFlagged(selectedCandidateId, submission.questionId)}
                            onChange={() => handleFlagChange(selectedCandidateId, submission.questionId)}
                          />
                          Flag Solution
                        </Button>
                      </div>
                    </div>
                    {isQuestionFlagged(selectedCandidateId, submission.questionId) && (
                      <Textarea className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl" placeholder="Flag reason" />
                    )}
                  </div>
                ))}
              </div>
            )}
            {selectedCandidateSubmission.mcq.length > 0 && (
              <div className="mb-8">
                <h2 className="text-lg font-semibold mb-4">MCQ Submissions</h2>
                {selectedCandidateSubmission.mcq.map((submission) => (
                  <div key={submission.questionId} className="space-y-4 mb-6 p-4 border rounded-lg">
                    <h3 className="font-medium">Question ID: {submission.questionId}</h3>
                    <div>
                      <h4 className="font-semibold mb-2">Selected Option:</h4>
                      <Card className="p-4 rounded-lg bg-[#eeeef0] dark:bg-[#27272a] shadow-none">
                        <div className="whitespace-pre-wrap p-2">Option {submission.answerOptions}</div>
                      </Card>
                    </div>
                    <div>
                      <h4 className="font-semibold mb-2">Result</h4>
                      <div className="flex items-center gap-5">
                        <div className="flex items-center gap-3">
                          <span>Points</span>
                          <input name="points" type="number" className="w-16" defaultValue={submission.score} />
                        </div>
                        <Button size="sm" variant="flat">
                          <input
                            name="flag"
                            type="checkbox"
                            checked={isQuestionFlagged(selectedCandidateId, submission.questionId)}
                            onChange={() => handleFlagChange(selectedCandidateId, submission.questionId)}
                          />
                          Flag Solution
                        </Button>
                      </div>
                    </div>
                    {isQuestionFlagged(selectedCandidateId, submission.questionId) && (
                      <Textarea className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl" placeholder="Flag reason" />
                    )}
                  </div>
                ))}
              </div>
            )}
            <div className="w-full flex justify-center">
              <Button className="my-3" color="primary">
                Save All Changes
              </Button>
            </div>
          </div>
        )}
      </div>
    </div>
  )
}