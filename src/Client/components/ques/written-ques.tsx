"use client";

import React, { useState, useMemo } from "react";
import { Button, Textarea, Checkbox, Card, Input } from "@heroui/react";
import PaginationButtons from "@/components/ui/pagination-button";
import { v4 as uuidv4 } from "uuid";
import toast from "react-hot-toast";
import api from "@/lib/api";
import { AxiosError } from "axios";

interface WrittenQuestion {
    id: string;
    question: string;
    points: number;
    difficultyType: string;
    isShortAnswer: boolean;
    isLongAnswer: boolean;
    shortAnswerText?: string;
    longAnswerText?: string;
}
interface FetchWrittenData {
    examId: string;
    writtenQuestions: {
        statementMarkdown: string;
        points: number;
        difficultyType: string;
        hasLongAnswer: boolean;
    }[];
}
interface WrittenQuestionFormProps {
    examId: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    existingQuestions: any[];
    onSaved: () => void;
}

export default function Component({
    examId,
    existingQuestions,
    onSaved,
}: WrittenQuestionFormProps) {
    const [writtenQuestions, setWrittenQuestions] = useState<WrittenQuestion[]>(
        existingQuestions.length > 0
            ? existingQuestions.map((q) => ({
                  id: uuidv4(),
                  question: q.statementMarkdown,
                  points: q.points,
                  difficultyType: q.difficultyType,
                  isShortAnswer: !q.hasLongAnswer,
                  isLongAnswer: q.hasLongAnswer,
              }))
            : [
                  {
                      id: uuidv4(),
                      question: "",
                      isShortAnswer: false,
                      isLongAnswer: false,
                      points: 0,
                      difficultyType: "",
                  },
              ]
    );

    const [currentPage, setCurrentPage] = useState(0);
    const [saveButton,setSaveButton]=useState(false)
    const questionsPerPage = 1;

    const handleAddWrittenQuestion = () => {
        setWrittenQuestions((prevQuestions) => {
            const newQuestions: WrittenQuestion[] = [
                ...prevQuestions,
                {
                    id: uuidv4(),
                    question: "",
                    isShortAnswer: false,
                    isLongAnswer: false,
                    points: 0,
                    difficultyType: "",
                },
            ];
            const newTotalPages = Math.ceil(
                newQuestions.length / questionsPerPage
            );
            setCurrentPage(newTotalPages - 1);
            return newQuestions;
        });
    };

    const handleQuestionChange = (questionId: string, newQuestion: string) => {
        setWrittenQuestions((prevQuestions) => {
            return prevQuestions.map((question) =>
                question.id === questionId
                    ? { ...question, question: newQuestion }
                    : question
            );
        });
    };
    const handleShortLongAnswer = (
        questionId: string,
        isShortChecked: boolean,
        isLongChecked: boolean
    ) => {
        setWrittenQuestions((prevQuestions) => {
            return prevQuestions.map((question) =>
                question.id === questionId
                    ? {
                          ...question,
                          isShortAnswer: isShortChecked,
                          isLongAnswer: isLongChecked,
                      }
                    : question
            );
        });
    };
    const handlePoints = (questionId: string, points: number) => {
        setWrittenQuestions((prevQuestions) => {
            return prevQuestions.map((question) =>
                question.id === questionId
                    ? { ...question, points: parseInt(points.toString()) }
                    : question
            );
        });
    };
    const handleDifficultyChange = (
        questionId: string,
        difficultyType: string
    ) => {
        setWrittenQuestions((prevQuestions) => {
            return prevQuestions.map((question) =>
                question.id === questionId
                    ? { ...question, difficultyType: difficultyType }
                    : question
            );
        });
    };
    const handleSaveWrittenQuestions = async () => {
           const hasMissingDifficulty = writtenQuestions.some(
                    (problem) =>
                        !problem.difficultyType || problem.difficultyType.trim() === ""
                );
                const hasMissingPoints = writtenQuestions.some(
                    (problem) => !problem.points );
                if (hasMissingDifficulty) {
                    toast.error("Please select a difficulty level");
                    return;
                }
                if (hasMissingPoints) {
                    toast.error("Please input points of the written question");
                    return;
                }
        try {
            const payload: FetchWrittenData = {
                examId: examId,
                writtenQuestions: writtenQuestions.map((q) => ({
                    statementMarkdown: q.question,
                    points: q.points,
                    difficultyType: q.difficultyType,
                    hasLongAnswer: q.isLongAnswer,
                })),
            };
            const response = await api.post(
                "/Questions/Written/Create",
                payload
            );
            if (response.status === 200) {
                toast.success("Written questions saved successfully!");
                onSaved();
                setWrittenQuestions([
                    {
                        id: uuidv4(),
                        question: "",
                        isShortAnswer: false,
                        isLongAnswer: false,
                        points: 0,
                        difficultyType: "",
                    },
                ]);
                setCurrentPage(0)
                setSaveButton(!saveButton)
            } else {
                toast.error("Failed to save written questions");
            }
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to save written questions");
        }
    };
    const totalPages = useMemo(
        () => Math.ceil(writtenQuestions.length / questionsPerPage),
        [writtenQuestions, questionsPerPage]
    );
    const currentQuestions = useMemo(
        () =>
            writtenQuestions.slice(
                currentPage * questionsPerPage,
                (currentPage + 1) * questionsPerPage
            ),
        [writtenQuestions, currentPage, questionsPerPage]
    );
    
    return (
        <div>
            <Card
                className={`flex flex-col items-center shadow-none bg-white dark:bg-[#18181b]`}
            >
                <h2 className="text-2xl mt-3">
                    Written Question : {currentPage + 1}
                </h2>

                {currentQuestions.map((question) => (
                    <div key={question.id} className="w-full">
                        <div className=" mr-8 w-full flex justify-end">
                            <select
                                className="rounded-md border p-2 dark:bg-[#1e293b] dark:text-gray-300 mx-2"
                                value={question.difficultyType}
                                onChange={(e) =>
                                    handleDifficultyChange(
                                        question.id,
                                        e.target.value
                                    )
                                }
                            >
                                <option value="">Select difficulty</option>
                                <option value="easy">Easy</option>
                                <option value="medium">Medium</option>
                                <option value="hard">Hard</option>
                            </select>
                        </div>
                        <div className="p-4 mx-5 rounded-lg mt-4">
                            <Textarea
                                label="Written Question"
                                name="question"
                                minRows={5}
                                className="bg-[#eeeef0] dark:bg-[#27272a] rounded-2xl"
                                value={question.question}
                                onChange={(e) =>
                                    handleQuestionChange(
                                        question.id,
                                        e.target.value
                                    )
                                }
                            />
                            <div className="flex items-center gap-4 mt-5">
                                <Input
                                    className="w-32"
                                    type="number"
                                    label="Points"
                                    value={question.points.toString()}
                                    onChange={(e) =>
                                        handlePoints(
                                            question.id,
                                            parseInt(e.target.value)
                                        )
                                    }
                                />
                                <label>
                                    <Checkbox
                                        isSelected={question.isShortAnswer}
                                        onChange={(e) =>
                                            handleShortLongAnswer(
                                                question.id,
                                                e.target.checked,
                                                e.target.checked
                                                    ? false
                                                    : question.isLongAnswer
                                            )
                                        }
                                        isDisabled={question.isLongAnswer}
                                    >
                                        Short Answer
                                    </Checkbox>
                                </label>

                                <label>
                                    <Checkbox
                                        isSelected={question.isLongAnswer}
                                        onChange={(e) =>
                                            handleShortLongAnswer(
                                                question.id,
                                                e.target.checked
                                                    ? false
                                                    : question.isShortAnswer,
                                                e.target.checked
                                            )
                                        }
                                        isDisabled={question.isShortAnswer}
                                    >
                                        Long Answer
                                    </Checkbox>
                                </label>
                            </div>
                        </div>
                    </div>
                ))}
                <div className="flex w-full justify-between items-center my-3 p-5">
                    <div className="flex items-center gap-2 ml-12">
                        <span>
                            Page {currentPage + 1} of {totalPages}
                        </span>
                        <PaginationButtons
                            currentIndex={currentPage + 1}
                            totalItems={totalPages}
                            onPrevious={() =>
                                setCurrentPage(Math.max(0, currentPage - 1))
                            }
                            onNext={() =>
                                setCurrentPage(
                                    Math.min(totalPages - 1, currentPage + 1)
                                )
                            }
                        />
                    </div>
                    <Button
                        color="primary"
                        className="mr-4"
                        isDisabled={saveButton}
                        onPress={handleSaveWrittenQuestions}
                    >
                        Save
                    </Button>
                </div>
            </Card>
            <div className="flex justify-center my-8">
                <Button onPress={handleAddWrittenQuestion}>
                    Add Written Question
                </Button>
            </div>
        </div>
    );
}
