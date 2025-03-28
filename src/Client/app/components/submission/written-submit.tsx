'use client'

import { Card } from '@heroui/react'

interface Question {
  id: number
  title: string
  description: string
  points: number
}
interface PageProps {
 readonly question: Question
 readonly setAnswers: React.Dispatch<React.SetStateAction<{ [key: number]: string | string[] }>>
 readonly answers: { [key: number]: string | string[] }
}

export default function WrittenSubmission({ question, setAnswers, answers }: PageProps) {
  return (
    <Card className=" p-5 shadow-none bg-white dark:bg-[#18181b]">
      <div className="w-full flex justify-between">
        <h2 className="text-lg font-semibold">{question.title}</h2>
        <p>points: {question.points}</p>
      </div>
      <div className={`space-y-4 p-4 rounded-lg `}>
        <p>{question.description}</p>
        <textarea
          className={`w-full bg-[#eeeef0] dark:bg-[#27272a] rounded-lg p-3`}
          placeholder="Type your answer here..."
          value={(answers[question.id] as string) || ''}
          rows={5}
          onChange={(e) =>
            setAnswers({
              ...answers,
              [question.id]: e.target.value,
            })
          }
        />
      </div>
    </Card>
  )
}
