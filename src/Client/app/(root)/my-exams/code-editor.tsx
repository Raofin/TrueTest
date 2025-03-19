'use client'

import React, { useEffect } from 'react'
import { Select, SelectItem } from '@heroui/react'
import Editor from '@monaco-editor/react'
import useTheme from '@/app/hooks/useTheme'

interface TestCase {
  input: string
  expectedOutput: string
  receivedOutput?: string
  status?: 'success' | 'error' | 'pending'
}

interface Question {
  id: number
  title: string
  description: string
  inputFormat: string
  outputFormat: string
  constraints: string
  examples: Array<{
    input: string
    output: string
    explanation?: string
  }>
  testCases: TestCase[]
}

interface CodeState {
  [key: string]: string
}

const languages = [
  {
    label: 'C++',
    value: 'cpp',
    defaultCode:
      '#include <bits/stdc++.h>\nusing namespace std;\n\nint main() {\n    // Your code here\n    return 0;\n}',
  },
  { label: 'Python', value: 'python', defaultCode: '# Your code here' },
  {
    label: 'Java',
    value: 'java',
    defaultCode:
      'public class Main {\n    public static void main(String[] args) {\n        // Your code here\n    }\n}',
  },
  { label: 'JavaScript', value: 'javascript', defaultCode: 'function solve(input) {\n    // Your code here\n}' },
  {
    label: 'C',
    value: 'c',
    defaultCode: '#include <stdio.h>\n\nint main() {\n    // Your code here\n    return 0;\n}',
  },
]

export default function App() {
  const [selectedLanguage, setSelectedLanguage] = React.useState('cpp')
  const [codeStates, setCodeStates] = React.useState<CodeState>({})
  const [selectedTestCase, setSelectedTestCase] = React.useState<number>(0)
  const Mode = useTheme()

  const question: Question = {
    id: 1,
    title: 'Alice and Bob',
    description: 'Write a program that takes two integers as input and prints their sum.',
    inputFormat: 'Two space-separated integers, a and b.',
    outputFormat: 'Print the sum of a and b.',
    constraints: '-10⁹ ≤ a, b ≤ 10⁹',
    examples: [
      {
        input: '3 7',
        output: '10',
        explanation: 'The sum of 3 and 7 is 10.',
      },
      {
        input: '99999 -99999',
        output: '0',
        explanation: 'The sum of 99999 and -99999 is 0.',
      },
    ],
    testCases: [
      { input: '3 7', receivedOutput: '10', expectedOutput: '10' },
      { input: '4 5', receivedOutput: '10', expectedOutput: '10' },
      { input: '99999 -99999', receivedOutput: '10', expectedOutput: '0' },
      { input: '1000000000 1000000000', receivedOutput: '2000000000', expectedOutput: '2000000000' },
    ],
  }

  useEffect(() => {
    const initialStates: CodeState = {}
    languages.forEach((lang) => {
      initialStates[lang.value] = lang.defaultCode
    })
    setCodeStates(initialStates)
  }, [])

  const handleCodeChange = (value: string | undefined) => {
    if (value !== undefined) {
      setCodeStates((prev) => ({
        ...prev,
        [selectedLanguage]: value,
      }))
    }
  }

  const handleTestCaseClick = (index: number) => {
    setSelectedTestCase(index)
  }

  const displayedTestCases = [question.testCases[selectedTestCase]]

  return (
    <div className="grid grid-cols-2 gap-4">
      <div className={`border-none rounded-lg p-4 ${Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'}`}>
        <h2 className="text-xl font-bold mb-3">{question.title}</h2>
        <div className="space-y-4">
          <div>
            <p>{question.description}</p>
          </div>
          <div>
            <h3 className="font-semibold">Input Format:</h3>
            <p>{question.inputFormat}</p>
          </div>
          <div>
            <h3 className="font-semibold">Output Format:</h3>
            <p>{question.outputFormat}</p>
          </div>
          <div>
            <h3 className="font-semibold">Constraints:</h3>
            <p>{question.constraints}</p>
          </div>
          <div>
            <h3 className="font-semibold">Examples:</h3>
            {question.examples.map((example, index) => (
              <div key={index} className="mt-2 p-3 rounded-lg">
                <div>
                  <span className="font-semibold">Input:</span> {example.input}
                </div>
                <div>
                  <span className="font-semibold">Output:</span> {example.output}
                </div>
                {example.explanation && (
                  <div>
                    <span className="font-semibold">Explanation:</span> {example.explanation}
                  </div>
                )}
              </div>
            ))}
          </div>
        </div>
      </div>
      <div className="border-none">
        <div className={`${Mode === 'dark' ? 'bg-[#18181b]' : 'bg-white'} px-3 rounded-lg h-[500px] mb-3`}>
          <div className={`flex w-full items-center justify-between pt-2`}>
            <p className="font-semibold">Code </p>
            <Select
               aria-label="Select Language" 
              selectedKeys={[selectedLanguage]}
              onChange={(e) => setSelectedLanguage(e.target.value)}
              className="w-40"
            >
              {languages.map((lang) => (
                <SelectItem key={lang.value} value={lang.value}>
                  {lang.label}
                </SelectItem>
              ))}
            </Select>
          </div>
          <div className={`m-3 rounded-lg overflow-hidden shadow-xl ${Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'}`}>
            <Editor
              height="420px"
              defaultLanguage={selectedLanguage}
              value={codeStates[selectedLanguage]}
              onChange={handleCodeChange}
              theme="vs-dark"
              options={{
                minimap: { enabled: false },
                fontSize: 14,
                lineNumbers: 'on',
                scrollBeyondLastLine: false,
                automaticLayout: true,
              }}
            />
          </div>
        </div>
        <div>
          <div className={`${Mode === 'dark' ? 'bg-[#18181b]' : 'bg-white'} p-4 rounded-lg`}>
            <div className="flex justify-between">
              <p className="font-bold">Test Cases</p>
              <div className="flex items-center gap-2">
                {question.testCases.map((_, index) => (
                  <button
                    key={index}
                    onClick={() => handleTestCaseClick(index)}
                    className={`w-8 h-8 rounded-lg flex items-center justify-center text-white 
                        ${_.receivedOutput === _.expectedOutput ? 'bg-green-500' : 'bg-red-500'}
                        ${selectedTestCase === index ? 'ring-2 ring-blue-500' : ''}`}
                  >
                    {index + 1}
                  </button>
                ))}
              </div>
            </div>
            <div className="grid grid-cols-3 gap-4 mt-3">
              <p className="p-2">Input</p>
              <p className="p-2">Expected Output</p>
              <p className="p-2">Received Output</p>
            </div>
            {displayedTestCases.map((testCase, index) => (
              <div key={index} className="p-2 rounded-lg ">
                <div className={`grid grid-cols-3 gap-4 h-[160px]`}>
                  <div className={`font-mono p-2 rounded-lg  ${Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'}`}>
                    {testCase.input}
                  </div>
                  <div className={`font-mono p-2 rounded-lg ${Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'}`}>
                    {testCase.expectedOutput}
                  </div>
                  <div className={`font-mono p-2 rounded-lg ${Mode === 'dark' ? 'bg-[#27272a]' : 'bg-white'}`}>
                    {testCase.receivedOutput}
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  )
}
