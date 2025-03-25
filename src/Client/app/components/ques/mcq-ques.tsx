'use client'

import React, { useState } from 'react'
import { Form, Button, Textarea, Card, CardBody, CardHeader, Checkbox, Input } from '@heroui/react'
import PaginationButtons from '@/app/components/ui/pagination-button'

interface MCQOption {
  id: number
  text: string
}

interface MCQQuestion {
  id?: string
  question: string
  options: MCQOption[]
  correctOption: number
}

export default function App() {
  const [questions, setQuestions] = useState<MCQQuestion[]>([
    {
      question: '',
      options: [
        { id: 1, text: '' },
        { id: 2, text: '' },
        { id: 3, text: '' },
        { id: 4, text: '' },
      ],
      correctOption: 1,
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
        correctOption: 1,
      },
    ])
    setCurrentPage(questions.length)
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
  }

  return (
    <div className="flex justify-center ">
      <Form id="#" className="w-full flex flex-col" onSubmit={handleSubmit}>
        {questions.length > 0 && (
          <Card key={currentPage} className="w-full shadow-none bg-white dark:bg-[#18181b]">
            <CardHeader className="flex flex-col gap-2 ">
              <h2 className="text-2xl my-3">MCQ Question : {currentPage + 1}</h2>
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
                  <div key={option.id} className="flex">
                    <Checkbox name="remember" size="sm"></Checkbox>
                    <Textarea
                     className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
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
              <Input className="w-32" type="number" placeholder="Points" />
              <div className='flex items-center gap-2 '>
              <span >
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

        <div className="w-full  my-3 text-center">
          <Button onPress={addNewQuestion}>
            Add MCQ Question
          </Button>
        </div>
      </Form>
    </div>
  )
}
