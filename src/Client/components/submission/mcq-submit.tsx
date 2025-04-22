'use client'

import { Card, Checkbox } from '@heroui/react';

interface MCQOption {
    option1: string;
    option2: string;
    option3: string;
    option4: string;
    isMultiSelect: boolean;
    answerOptions: string;
}

interface MCQQuestion {
    questionId: string;
    examId: string;
    questionType: string;
    statementMarkdown: string;
    score: number;
    difficultyType: string;
    mcqOption: MCQOption;
}

interface PageProps {
    readonly question: MCQQuestion;
    readonly setAnswers: React.Dispatch<React.SetStateAction<{ [key: string]: string | string[] }>>;
    readonly answers: { [key: string]: string | string[] };
    readonly options: string[];
}

export default function MCQSubmission({ question, answers, setAnswers, options }: PageProps) {
  const handleCheckboxChange = (questionId: string, option: string, isChecked: boolean) => {
    setAnswers((prevAnswers) => {
      const currentAnswers = prevAnswers[questionId] ?? [];
      let updatedAnswers: string[] = [];
      if (isChecked) {
        updatedAnswers = [...(currentAnswers as string[]), option];
      } else {
        updatedAnswers = (currentAnswers as string[]).filter((o) => o !== option);
      }
      return { ...prevAnswers, [questionId]: updatedAnswers };
    });
  };

  return (
    <Card className="p-5 shadow-none bg-white dark:bg-[#18181b]">
     
      <div className={`space-y-4 p-4 rounded-lg `}>
        <p>{question.statementMarkdown}</p>
        {options.map((option) => (
          <div key={option} className="flex items-center gap-2 p-3 rounded-lg hover:bg-white/5">
            <Checkbox
              value={option}
              isSelected={Array.isArray(answers[question.questionId]) && 
                        (answers[question.questionId] as string[]).includes(option)}
              onValueChange={(isChecked) => handleCheckboxChange(question.questionId, option, isChecked)}
            >
              {option}
            </Checkbox>
          </div>
        ))}
      </div>
    </Card>
  );
}