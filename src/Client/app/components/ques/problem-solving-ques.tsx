'use client'

import React, { useState } from 'react'
import { Form, Button, Textarea, Card, Input } from '@heroui/react'
import { Icon } from '@iconify/react'
import toast from 'react-hot-toast'
import PaginationButtons from '@/app/components/ui/pagination-button'
import MDEditor from '@uiw/react-md-editor'
import he from 'he'

interface TestCase {
  id?: string
  input: string
  output: string
}

interface Problem {
  id?: string
  question: string
  testCases: TestCase[]
}

export default function ProblemSolvingFormp() {
  const [problems, setProblems] = useState<Problem[]>([{ question: '', testCases: [{ input: '', output: '' }] }])
  const [currentPage, setCurrentPage] = useState(0)
  const problemsPerPage = 1
  const [value, setValue] = React.useState('"Hello, World!"')
  const escaped: string = he.escape(value)
  const unescaped: string = he.unescape(escaped)
  console.log(unescaped)

  const addTestCase = (problemIndex: number) => {
    setProblems((prevProblems) => {
      const updatedProblems = [...prevProblems]
      updatedProblems[problemIndex] = {
        ...updatedProblems[problemIndex],
        testCases: [...updatedProblems[problemIndex].testCases, { input: '', output: '' }],
      }
      return updatedProblems
    })
    setValue('')
  }

  const handleDelete = (problemIndex: number, testCaseIndex: number) => {
    setProblems((prevProblems) => {
      const updatedProblems = [...prevProblems]
      updatedProblems[problemIndex] = {
        ...updatedProblems[problemIndex],
        testCases: updatedProblems[problemIndex].testCases.filter((_, i) => i !== testCaseIndex),
      }
      return updatedProblems
    })

    toast.success('Test case deleted successfully')
  }

  const handleRefresh = (problemIndex: number, testCaseIndex: number) => {
    setProblems((prevProblems) => {
      const updatedProblems = [...prevProblems]
      updatedProblems[problemIndex].testCases[testCaseIndex] = { input: '', output: '' }
      return updatedProblems
    })

    toast.success('Test case refreshed successfully')
  }

  const handleAddProblem = () => {
    setProblems((prevProblems) => {
      const newProblems = [...prevProblems, { question: '', testCases: [{ input: '', output: '' }] }]
      const newTotalPages = Math.ceil(newProblems.length / problemsPerPage)
      setCurrentPage(newTotalPages - 1)
      return newProblems
    })
  }

  const handleTestCaseInputChange = (problemIndex: number, testCaseIndex: number, newInput: string) => {
    setProblems((prevProblems) => {
      const updatedProblems = [...prevProblems]
      updatedProblems[problemIndex].testCases[testCaseIndex] = {
        ...updatedProblems[problemIndex].testCases[testCaseIndex],
        input: newInput,
      }
      return updatedProblems
    })
  }

  const handleTestCaseOutputChange = (problemIndex: number, testCaseIndex: number, newOutput: string) => {
    setProblems((prevProblems) => {
      const updatedProblems = [...prevProblems]
      updatedProblems[problemIndex].testCases[testCaseIndex] = {
        ...updatedProblems[problemIndex].testCases[testCaseIndex],
        output: newOutput,
      }
      return updatedProblems
    })
  }

  const totalPages = Math.ceil(problems.length / problemsPerPage)
  const currentProblems = problems.slice(currentPage * problemsPerPage, (currentPage + 1) * problemsPerPage)

  const handleSaveAll = () => {
    // setIsModalOpen(true);
  }
  return (
    <div>
    <Card className="border-none shadow-none bg-white dark:bg-[#18181b]">
      <h2 className="flex justify-center text-2xl my-3">Problem Solving Question : {currentPage + 1}</h2>
      <div className={`flex justify-center p-4`}>
        <Form id="#" className="w-full flex flex-col gap-4 p-5 border-none">
          {currentProblems.map((problem, problemIndex) => (
            <div key={problemIndex} className="  rounded-lg w-full flex flex-col justify-center items-center">
              <MDEditor
                className="rounded-lg w-full"
                value={value}
                onChange={(val) => {
                  setValue(val || '')
                }}
              />
              {problem.testCases.map((testCase, testCaseIndex) => (
                <div key={testCaseIndex} className="w-full flex gap-2 mt-2">
                  <div className="flex flex-col gap-2">
                    <Button
                      isIconOnly
                      variant="flat"
                      isDisabled={problem.testCases.length === 1}
                      onPress={() => handleDelete(currentPage * problemsPerPage + problemIndex, testCaseIndex)}
                    >
                      <Icon icon="lucide:trash-2" width={20} />
                    </Button>
                    <Button
                      isIconOnly
                      variant="flat"
                      onPress={() => handleRefresh(currentPage * problemsPerPage + problemIndex, testCaseIndex)}
                    >
                      <Icon icon="lucide:refresh-cw" width={20} />
                    </Button>
                  </div>

                  <div className="w-full flex gap-3">
                    <Textarea
                      label="Input"
                      value={testCase.input}
                      className="flex-1 bg-[#f4f4f5] dark:bg-[#27272a] rounded-2xl"
                      onChange={(e: { target: { value: string } }) =>
                        handleTestCaseInputChange(
                          currentPage * problemsPerPage + problemIndex,
                          testCaseIndex,
                          e.target.value
                        )
                      }
                    />
                    <Textarea
                      label="Expected Output"
                      value={testCase.output}
                      className="flex-1 bg-[#f4f4f5] dark:bg-[#27272a] rounded-2xl"
                      onChange={(e: { target: { value: string } }) =>
                        handleTestCaseOutputChange(
                          currentPage * problemsPerPage + problemIndex,
                          testCaseIndex,
                          e.target.value
                        )
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
                onPress={() => addTestCase(currentPage * problemsPerPage)}
              >
                Add Test Case
              </Button>
              <Button color="primary" onPress={handleSaveAll}>
                Save
              </Button>
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
