'use client'

import React, { useState, useMemo } from 'react'
import { Form, Button, Textarea, Card, Input } from '@heroui/react'
import { Icon } from '@iconify/react'
import toast from 'react-hot-toast'
import PaginationButtons from '@/app/components/ui/pagination-button'
import he from 'he'
import { v4 as uuidv4 } from 'uuid'
import MdEditor from '../katex-mermaid'

interface TestCase {
  id: string
  input: string
  output: string
}

interface Problem {
  id: string
  question: string
  testCases: TestCase[]
}

export default function ProblemSolvingFormp() {
  const [problems, setProblems] = useState<Problem[]>([
    { id: uuidv4(), question: '', testCases: [{ id: uuidv4(), input: '', output: '' }] },
  ])
  const [currentPage, setCurrentPage] = useState(0)
  const problemsPerPage = 1
  const [value, setValue] = React.useState('"Hello, World!"')
  const escaped: string = he.escape(value)
  const unescaped: string = he.unescape(escaped)
  console.log(unescaped)

  const addTestCase = (problemId: string) => {
    setProblems((prevProblems) => {
      return prevProblems.map((problem) => {
        if (problem.id === problemId) {
          return {
            ...problem,
            testCases: [...problem.testCases, { id: uuidv4(), input: '', output: '' }],
          }
        }
        return problem
      })
    })
    setValue('')
  }

  const handleDelete = (problemId: string, testCaseId: string) => {
    setProblems((prevProblems) => {
      return prevProblems.map((problem) => {
        if (problem.id === problemId) {
          return {
            ...problem,
            testCases: problem.testCases.filter((testCase) => testCase.id !== testCaseId),
          }
        }
        return problem
      })
    })

    toast.success('Test case deleted successfully')
  }

  const handleRefresh = (problemId: string, testCaseId: string) => {
    setProblems((prevProblems) => {
      return prevProblems.map((problem) => {
        if (problem.id === problemId) {
          return {
            ...problem,
            testCases: problem.testCases.map((testCase) =>
              testCase.id === testCaseId ? { ...testCase, input: '', output: '' } : testCase
            ),
          }
        }
        return problem
      })
    })

    toast.success('Test case refreshed successfully')
  }

  const handleAddProblem = () => {
    setProblems((prevProblems) => {
      const newProblems = [
        ...prevProblems,
        { id: uuidv4(), question: '', testCases: [{ id: uuidv4(), input: '', output: '' }] },
      ]
      const newTotalPages = Math.ceil(newProblems.length / problemsPerPage)
      setCurrentPage(newTotalPages - 1)
      return newProblems
    })
  }

  const handleTestCaseInputChange = (problemId: string, testCaseId: string, newInput: string) => {
    setProblems((prevProblems) => {
      return prevProblems.map((problem) => {
        if (problem.id === problemId) {
          return {
            ...problem,
            testCases: problem.testCases.map((testCase) =>
              testCase.id === testCaseId ? { ...testCase, input: newInput } : testCase
            ),
          }
        }
        return problem
      })
    })
  }

  const handleTestCaseOutputChange = (problemId: string, testCaseId: string, newOutput: string) => {
    setProblems((prevProblems) => {
      return prevProblems.map((problem) => {
        if (problem.id === problemId) {
          return {
            ...problem,
            testCases: problem.testCases.map((testCase) =>
              testCase.id === testCaseId ? { ...testCase, output: newOutput } : testCase
            ),
          }
        }
        return problem
      })
    })
  }

  const totalPages = useMemo(() => Math.ceil(problems.length / problemsPerPage), [problems, problemsPerPage])
  const currentProblems = useMemo(
    () => problems.slice(currentPage * problemsPerPage, (currentPage + 1) * problemsPerPage),
    [problems, currentPage, problemsPerPage]
  )

  return (
    <div>
      <Card className="border-none shadow-none bg-white dark:bg-[#18181b]">
        <h2 className="flex justify-center text-2xl my-3">Problem Solving Question : {currentPage + 1}</h2>
        <div className={`flex justify-center p-4`}>
          <Form id="#" className="w-full flex flex-col gap-4 p-5 border-none">
            {currentProblems.map((problem) => (
              <div key={problem.id} className="rounded-lg w-full flex flex-col justify-center items-center">
                <MdEditor/>
                {problem.testCases.map((testCase) => (
                  <div key={testCase.id} className="w-full flex gap-2 mt-2">
                    <div className="flex flex-col gap-2">
                      <Button
                        isIconOnly
                        variant="flat"
                        isDisabled={problem.testCases.length === 1}
                        onPress={() => handleDelete(problem.id, testCase.id)}
                      >
                        <Icon icon="lucide:trash-2" width={20} />
                      </Button>
                      <Button isIconOnly variant="flat" onPress={() => handleRefresh(problem.id, testCase.id)}>
                        <Icon icon="lucide:refresh-cw" width={20} />
                      </Button>
                    </div>

                    <div className="w-full flex gap-3">
                      <Textarea
                        label="Input"
                        value={testCase.input}
                        className="flex-1 bg-[#f4f4f5] dark:bg-[#27272a] rounded-2xl"
                        onChange={(e: { target: { value: string } }) =>
                          handleTestCaseInputChange(problem.id, testCase.id, e.target.value)
                        }
                      />
                      <Textarea
                        label="Expected Output"
                        value={testCase.output}
                        className="flex-1 bg-[#f4f4f5] dark:bg-[#27272a] rounded-2xl"
                        onChange={(e: { target: { value: string } }) =>
                          handleTestCaseOutputChange(problem.id, testCase.id, e.target.value)
                        }
                      />
                    </div>
                  </div>
                ))}
              </div>
            ))}
            <div className="flex w-full justify-between items-center mt-4 ">
              <Input className="w-32" type="number" placeholder="Points" />
              <div className="flex gap-2 items-center">
                <span>
                  Page {currentPage + 1} of {totalPages}
                </span>
                <PaginationButtons
                  currentIndex={currentPage + 1}
                  totalItems={totalPages}
                  onPrevious={() => setCurrentPage(currentPage - 1)}
                  onNext={() => setCurrentPage(currentPage + 1)}
                />
              </div>
              <div className="flex gap-2 items-center ">
                <Button
                  variant="flat"
                  startContent={<Icon icon="lucide:plus" />}
                  onPress={() => addTestCase(currentProblems[0].id)}
                >
                  Add Test Case
                </Button>
                <Button color="primary">Save</Button>
              </div>
            </div>
          </Form>
        </div>
      </Card>
      <div className="flex w-full justify-center mt-5">
        <Button onPress={handleAddProblem}>Add Problem Solving Question</Button>
      </div>
    </div>
  )
}
