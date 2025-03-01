'use client'
import React, { useState } from "react";
import { Form, Button, Textarea, Checkbox } from "@heroui/react";

interface WrittenQuestion {
    id?: string;
    question: string;
    isShortAnswer: boolean;
    isLongAnswer: boolean;
}

export default function App() {
    const [writtenQuestions, setWrittenQuestions] = useState<WrittenQuestion[]>([
        { question: "", isShortAnswer: false, isLongAnswer: false }
    ]);
    const [currentPage, setCurrentPage] = useState(0);
    const questionsPerPage = 1;

    const handleAddWrittenQuestion = () => {
        setWrittenQuestions(prevQuestions => {
            const newQuestions: WrittenQuestion[] = [...prevQuestions, { question: "", isShortAnswer: false, isLongAnswer: false }];
            const newTotalPages = Math.ceil(newQuestions.length / questionsPerPage);
            setCurrentPage(newTotalPages - 1);
            return newQuestions;
        });
    };

    const handleQuestionChange = (questionIndex: number, newQuestion: string) => {
        setWrittenQuestions(prevQuestions => {
            const updatedQuestions = [...prevQuestions];
            updatedQuestions[questionIndex] = { ...updatedQuestions[questionIndex], question: newQuestion };
            return updatedQuestions;
        });
    };

    const handleShortAnswerChange = (questionIndex: number, isChecked: boolean) => {
        setWrittenQuestions(prevQuestions => {
            const updatedQuestions = [...prevQuestions];
            updatedQuestions[questionIndex] = { ...updatedQuestions[questionIndex], isShortAnswer: isChecked };
            return updatedQuestions;
        });
    };

    const handleLongAnswerChange = (questionIndex: number, isChecked: boolean) => {
        setWrittenQuestions(prevQuestions => {
            const updatedQuestions = [...prevQuestions];
            updatedQuestions[questionIndex] = { ...updatedQuestions[questionIndex], isLongAnswer: isChecked };
            return updatedQuestions;
        });
    };

    const totalPages = Math.ceil(writtenQuestions.length / questionsPerPage);
    const currentQuestions = writtenQuestions.slice(currentPage * questionsPerPage, (currentPage + 1) * questionsPerPage);

    const goToPreviousPage = () => {
        if (currentPage > 0) {
            setCurrentPage(currentPage - 1);
        }
    };

    const goToNextPage = () => {
        if (currentPage < totalPages - 1) {
            setCurrentPage(currentPage + 1);
        }
    };

    return (
        <div className="flex flex-col items-center p-4 shadow m-10">
            <h2 className="text-2xl">Written Question</h2>
            <div className="w-full max-w-3xl">
                {currentQuestions.map((question, questionIndex) => (
                    <div key={questionIndex} className="border p-4 rounded-lg shadow-md mt-4">
                        <Textarea
                            label="Written Question"
                            name="question"
                            minRows={4}
                            variant="bordered"
                            value={question.question}
                            onChange={(e) => handleQuestionChange(currentPage * questionsPerPage + questionIndex, e.target.value)}
                        />

                        <div className="flex items-center gap-4">
                            <label>
                                <Checkbox checked={question.isShortAnswer} onChange={(e) => handleShortAnswerChange(currentPage * questionsPerPage + questionIndex, e.target.checked)} /> Short Answer
                            </label>
                            <label>
                                <Checkbox checked={question.isLongAnswer} onChange={(e) => handleLongAnswerChange(currentPage * questionsPerPage + questionIndex, e.target.checked)} /> Long Answer
                            </label>
                        </div>
                    </div>
                ))}

                <div className="flex justify-end mt-10">
                    <Button color="primary" onPress={handleAddWrittenQuestion}>
                        Add Written Question
                    </Button>
                </div>

                <div className="flex justify-center items-center mt-4">
                    <Button onPress={goToPreviousPage} disabled={currentPage === 0}>Previous</Button>
                    <span className="mx-2">Page {currentPage + 1} of {totalPages}</span>
                    <Button onPress={goToNextPage} disabled={currentPage === totalPages - 1}>Next</Button>
                </div>
            </div>
        </div>
    );
}