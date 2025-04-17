'use client'

import { Card } from '@heroui/react'

interface WrittenQuestion { 
  questionId: string;
  examId: string;
  questionType: string;
  hasLongAnswer: boolean;
  statementMarkdown: string;
  score: number;
  difficultyType: string;
}

interface PageProps {
  readonly question: WrittenQuestion; 
  readonly setAnswers: React.Dispatch<React.SetStateAction<{ [key: string]: string | string[] }>>; 
  readonly answers: { [key: string]: string | string[] };
}

export default function WrittenSubmission({ question, setAnswers, answers }: PageProps) {
  return (
    <Card className=" p-5 shadow-none bg-white dark:bg-[#18181b]">
      <div className="w-full flex justify-between">
        <h2 className="text-lg font-semibold">#Question :</h2>
        <p>points: {question.score}</p> 
      </div>
      <div className={`space-y-4 p-4 rounded-lg `}>
        <p>{question.statementMarkdown}</p>
        <textarea
          className={`w-full bg-[#eeeef0] dark:bg-[#27272a] rounded-lg p-3`}
          placeholder="Type your answer here..."
          value={(answers[question.questionId] as string) || ''} 
          rows={5}
          onChange={(e) =>
            setAnswers({
              ...answers,
              [question.questionId]: e.target.value,
            })
          }
        />
      </div>
    </Card>
  )
}