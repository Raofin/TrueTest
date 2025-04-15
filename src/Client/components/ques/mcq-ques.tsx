'use client'

import React, { useState } from 'react'
import { Form, Button, Textarea, Card, CardBody, CardHeader, Checkbox, Input } from '@heroui/react'
import PaginationButtons from '@/components/ui/pagination-button'
import api from '@/lib/api'
import toast from 'react-hot-toast'
import { AxiosError } from 'axios'

interface MCQOption {
  id: number
  text: string
}

interface MCQQuestion {
  question: string
  points: number
  options: MCQOption[]
  correctOptions: number[]
  difficultyType: string
}

interface FetchMcqData {
  examId: string
  mcqQuestions: {
    statementMarkdown: string
    points: number
    difficultyType: string
    mcqOption: {
      option1: string
      option2: string
      option3: string
      option4: string
      isMultiSelect: boolean
      answerOptions: string
    }
  }[]
}

export default function App({examId }: {readonly examId: string }) {
  const [questions, setQuestions] = useState<MCQQuestion[]>([
    {
      question: '',
      options: [
        { id: 1, text: '' },
        { id: 2, text: '' },
        { id: 3, text: '' },
        { id: 4, text: '' },
      ],
      correctOptions: [],
      points: 0,
      difficultyType: '',
    },
  ])

  const [currentPage, setCurrentPage] = useState(0)

  const handleQuestionChange = (index: number, value: string) => {
    const newQuestions = [...questions]
    newQuestions[index].question = value
    setQuestions(newQuestions)
  }

  const handleOptionChange = (questionIndex: number, optionId: number, value: string) => {
    const newQuestions = [...questions]
    const optionIndex = newQuestions[questionIndex].options.findIndex((opt) => opt.id === optionId)
    newQuestions[questionIndex].options[optionIndex].text = value
    setQuestions(newQuestions)
  }

  const handleCorrectOptionChange = (questionIndex: number, optionId: number) => {
    const newQuestions = [...questions]
    const currentQuestion = newQuestions[questionIndex]
    const currentCorrectOptions = currentQuestion.correctOptions
    const optionIndex = currentCorrectOptions.indexOf(optionId)
    if (optionIndex === -1) {
      currentCorrectOptions.push(optionId)
    } else {
      currentCorrectOptions.splice(optionIndex, 1)
    }

    setQuestions(newQuestions)
  }

  const addNewQuestion = () => {
    setQuestions([
      ...questions,
      {
        question: '',
        options: [
          { id: 1, text: '' },
          { id: 2, text: '' },
          { id: 3, text: '' },
          { id: 4, text: '' },
        ],
        correctOptions: [],
        points: 0,
        difficultyType: '',
      },
    ])
    setCurrentPage(questions.length)
  }

  const handlePoints = (questionIndex: number, value: string) => {
    const newQuestions = [...questions]
    newQuestions[questionIndex].points = parseInt(value) || 0
    setQuestions(newQuestions)
  }

  const handleDifficultyChange = (questionIndex: number, value: string) => {
    const newQuestions = [...questions]
    newQuestions[questionIndex].difficultyType = value
    setQuestions(newQuestions)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    const payload: FetchMcqData = {
      examId: examId,
      mcqQuestions: questions.map((q) => ({
        statementMarkdown: q.question,
        points: q.points,
        difficultyType: q.difficultyType,
        mcqOption: {
          option1: q.options[0].text,
          option2: q.options[1].text,
          option3: q.options[2].text,
          option4: q.options[3].text,
          isMultiSelect: q.correctOptions.length > 1,
          answerOptions: q.correctOptions.join(','),
        },
      })),
    }

    try {
      const resp = await api.post('/Questions/Mcq/Create', payload)
      if (resp.status === 200) {
        toast.success('MCQ questions saved successfully!')
      } else {
        toast.error('Failed to save MCQ questions')
      }
    } catch (error) {
      const err = error as AxiosError
      toast.error(err.message ?? 'Failed to save problems')
    }
  }

  return (
    <div className="flex justify-center ">
      <Form id="#" className="w-full flex flex-col" onSubmit={handleSubmit}>
        {questions.length > 0 && (
          <Card key={currentPage} className="w-full shadow-none bg-white dark:bg-[#18181b]">
            <CardHeader className="flex flex-col gap-2 ">
              <h2 className="text-2xl my-3">MCQ Question : {currentPage + 1}</h2>
              <div className="w-full flex justify-end mr-5">
                <select
                  className="rounded-md border p-2 dark:bg-[#1e293b] dark:text-gray-300"
                  value={questions[currentPage].difficultyType}
                  onChange={(e) => handleDifficultyChange(currentPage, e.target.value)}
                >
                  <option value="">Select Difficulty</option>
                  <option value="easy">Easy</option>
                  <option value="medium">Medium</option>
                  <option value="hard">Hard</option>
                </select>
              </div>
            </CardHeader>
            <CardBody className="flex flex-col gap-4 p-8">
              <Textarea
                label="mcq question"
                minRows={5}
                value={questions[currentPage].question}
                className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
                onChange={(e: { target: { value: string } }) => handleQuestionChange(currentPage, e.target.value)}
              />
              <div className="grid grid-cols-2 gap-4">
                {questions[currentPage].options.map((option) => (
                  <div key={option.id} className="flex items-center gap-2">
                    <Checkbox
                      isSelected={questions[currentPage].correctOptions.includes(option.id)}
                      onChange={() => handleCorrectOptionChange(currentPage, option.id)}
                      name={`option-${option.id}`}
                      size="sm"
                    />
                    <Textarea
                      className="bg-[#eeeef0] dark:[#71717a] rounded-2xl flex-grow"
                      label={`Option ${option.id}`}
                      value={option.text}
                      isRequired={option.id === 1 || option.id === 2}
                      onChange={(e: { target: { value: string } }) =>
                        handleOptionChange(currentPage, option.id, e.target.value)
                      }
                    />
                  </div>
                ))}
              </div>
            </CardBody>
            <div className="w-full flex justify-between px-8 py-5">
              <Input
                className="w-32"
                type="number"
                placeholder="Points"
                value={questions[currentPage].points.toString()}
                onChange={(e) => handlePoints(currentPage, e.target.value)}
                min="0"
              />
              <div className="flex items-center gap-2 ">
                <span>
                  Page {currentPage + 1} of {questions.length}
                </span>
                <PaginationButtons
                  currentIndex={currentPage + 1}
                  totalItems={questions.length}
                  onPrevious={() => setCurrentPage((prev) => Math.max(prev - 1, 0))}
                  onNext={() => setCurrentPage((prev) => Math.min(prev + 1, questions.length - 1))}
                />
              </div>

              <Button color="primary" type="submit">
                Save
              </Button>
            </div>
          </Card>
        )}

        <div className="w-full my-3 text-center">
          <Button onPress={addNewQuestion}>Add MCQ Question</Button>
        </div>
      </Form>
    </div>
  )
}
