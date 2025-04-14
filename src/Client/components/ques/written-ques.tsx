'use client';

import React, { useState, useMemo } from 'react';
import { Button, Textarea, Checkbox, Card, Input } from '@heroui/react';
import PaginationButtons from '@/components/ui/pagination-button';
import { v4 as uuidv4 } from 'uuid';  

interface WrittenQuestion {
  id: string; 
  question: string;
  isShortAnswer: boolean;
  isLongAnswer: boolean;
  shortAnswerText?: string;
  longAnswerText?: string;
}

export default function App() {
  const [writtenQuestions, setWrittenQuestions] = useState<WrittenQuestion[]>([
    { id: uuidv4(), question: '', isShortAnswer: false, isLongAnswer: false },
  ]);
  const [currentPage, setCurrentPage] = useState(0);
  const questionsPerPage = 1;

  const handleAddWrittenQuestion = () => {
    setWrittenQuestions((prevQuestions) => {
      const newQuestions: WrittenQuestion[] = [
        ...prevQuestions,
        { id: uuidv4(), question: '', isShortAnswer: false, isLongAnswer: false },
      ];
      const newTotalPages = Math.ceil(newQuestions.length / questionsPerPage);
      setCurrentPage(newTotalPages - 1);
      return newQuestions;
    });
  };

  const handleQuestionChange = (questionId: string, newQuestion: string) => {
    setWrittenQuestions((prevQuestions) => {
      return prevQuestions.map((question) =>
        question.id === questionId ? { ...question, question: newQuestion } : question
      );
    });
  };

  const handleShortAnswerChange = (questionId: string, isChecked: boolean) => {
    setWrittenQuestions((prevQuestions) => {
      return prevQuestions.map((question) =>
        question.id === questionId
          ? {
              ...question,
              isShortAnswer: isChecked,
              isLongAnswer: !isChecked,
              shortAnswerText: isChecked ? '' : undefined,
              longAnswerText: undefined,
            }
          : question
      );
    });
  };

  const handleLongAnswerChange = (questionId: string, isChecked: boolean) => {
    setWrittenQuestions((prevQuestions) => {
      return prevQuestions.map((question) =>
        question.id === questionId
          ? {
              ...question,
              isLongAnswer: isChecked,
              isShortAnswer: !isChecked,
              longAnswerText: isChecked ? '' : undefined,
              shortAnswerText: undefined,
            }
          : question
      );
    });
  };

  const totalPages = useMemo(() => Math.ceil(writtenQuestions.length / questionsPerPage), [writtenQuestions, questionsPerPage]);
  const currentQuestions = useMemo(() => writtenQuestions.slice(currentPage * questionsPerPage, (currentPage + 1) * questionsPerPage), [writtenQuestions, currentPage, questionsPerPage]);

  return (
    <div>
      <Card className={`flex flex-col items-center shadow-none bg-white dark:bg-[#18181b]`}>
        <h2 className="text-2xl mt-3">Written Question : {currentPage + 1}</h2>
        <div className="w-full">
          {currentQuestions.map((question) => (
            <div key={question.id} className="p-4 mx-5 rounded-lg mt-4">
              <Textarea
                label="Written Question"
                name="question"
                minRows={5}
                className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
                value={question.question}
                onChange={(e: { target: { value: string } }) => handleQuestionChange(question.id, e.target.value)}
              />

              <div className="flex items-center gap-4 mt-5">
                <Input className="w-32" type="number" placeholder="Points" />
                <label>
                  <Checkbox
                    checked={question.isShortAnswer}
                    onChange={(e: { target: { checked: boolean } }) => handleShortAnswerChange(question.id, e.target.checked)}
                    isDisabled={question.isLongAnswer}
                  >
                    Short Answer
                  </Checkbox>
                </label>
                <label>
                  <Checkbox
                    checked={question.isLongAnswer}
                    onChange={(e: { target: { checked: boolean } }) => handleLongAnswerChange(question.id, e.target.checked)}
                    isDisabled={question.isShortAnswer}
                  >
                    Long Answer
                  </Checkbox>
                </label>
              </div>
            </div>
          ))}

          <div className="flex w-full justify-between items-center my-3 p-5">
            <div></div>
            <div className="flex items-center gap-2 ml-12">
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
            <Button color="primary" className="mr-4">
              Save
            </Button>
          </div>
        </div>
      </Card>
      <div className="flex justify-center my-8">
        <Button onPress={handleAddWrittenQuestion}>Add Written Question</Button>
      </div>
    </div>
  );
}