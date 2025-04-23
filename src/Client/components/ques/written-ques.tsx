"use client";

import React, { useState, useMemo, useEffect } from "react";
import { Button, Textarea, Checkbox, Card, Input } from "@heroui/react";
import PaginationButtons from "@/components/ui/pagination-button";
import { v4 as uuidv4 } from "uuid";
import toast from "react-hot-toast";
import api from "@/lib/api";
import { AxiosError } from "axios";

interface WrittenQuestion {
    id: string;
    questionId: string;
    question: string;
    points: number;
    difficultyType: string;
    isShortAnswer: boolean;
    isLongAnswer: boolean;
}
interface ExistingQuestion {
    questionId: string;
    statementMarkdown: string;
    points: number;
    difficultyType: string;
    hasLongAnswer: boolean;
}
interface WrittenQuestionFormProps {
    readonly examId: string;
    readonly existingQuestions: ExistingQuestion[];
    readonly onSaved: () => void;
    readonly writtenPoints: (points: number) => void;
}

export default function Component({
    examId,
    existingQuestions,
    onSaved,
    writtenPoints,
}: WrittenQuestionFormProps) {
    const [writtenQuestions, setWrittenQuestions] = useState<WrittenQuestion[]>(
        existingQuestions.length > 0
            ? existingQuestions.map((q) => ({
                  id: uuidv4(),
                  questionId: q.questionId, 
                  question: q.statementMarkdown,
                  points: q.points || 0,
                  difficultyType: q.difficultyType || "Easy",
                  isShortAnswer: !q.hasLongAnswer,
                  isLongAnswer: q.hasLongAnswer,
              }))
            : [
                  {
                      id: uuidv4(),
                      questionId: "",
                      question: "",
                      points: 0,
                      difficultyType: "Easy",
                      isShortAnswer: false,
                      isLongAnswer: false,
                  },
              ]
    );
    const [currentPage, setCurrentPage] = useState(0);
    const [saveButton, setSaveButton] = useState(false);
    const questionsPerPage = 1;
    useEffect(() => {
        const total = writtenQuestions.reduce(
            (sum, problem) => sum + (problem.points || 0),
            0
        );
        writtenPoints(total);
    }, [writtenQuestions, writtenPoints]);
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
                    difficultyType: "Easy",
                    questionId: "",
                },
            ];
            const newTotalPages = Math.ceil(
                newQuestions.length / questionsPerPage
            );
            setCurrentPage(newTotalPages - 1);
            return newQuestions;
        });
    };
    const handleDeleteQuestion = async (
        questionId: string,
        backendId: string
    ) => {
        try {
            if (backendId) {
                await api.delete(`/Questions/Written/Delete/${backendId}`);
            }
            setWrittenQuestions((prev) =>
                prev.filter((q) => q.id !== questionId)
            );
            if (
                currentPage >=
                Math.ceil((writtenQuestions.length - 1) / questionsPerPage)
            ) {
                setCurrentPage(Math.max(0, currentPage - 1));
            }

            toast.success("Question deleted successfully");
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to delete question");
        }
    };
    const handleUpdateQuestion = async (questionId: string) => {
        const questionToUpdate = writtenQuestions.find(
            (q) => q.id === questionId
        );
        if (!questionToUpdate) return;

        try {
            if (!questionToUpdate.questionId) {
                const response = await api.post("/Questions/Written/Create", {
                    examId: examId,
                    writtenQuestions: [
                        {
                            statementMarkdown: questionToUpdate.question,
                            points: questionToUpdate.points,
                            difficultyType: questionToUpdate.difficultyType,
                            hasLongAnswer: questionToUpdate.isLongAnswer,
                        },
                    ],
                });

                if (response.status === 200) {
                    setWrittenQuestions((prev) =>
                        prev.map((q) =>
                            q.id === questionId
                                ? {
                                      ...q,
                                      questionId: response.data[0]?.questionId,
                                  }
                                : q
                        )
                    );
                    toast.success("Question saved successfully!");
                }
            } else {
                await api.patch("/Questions/Written/Update", {
                    questionId: questionToUpdate.questionId,
                    statementMarkdown: questionToUpdate.question,
                    points: questionToUpdate.points,
                    difficultyType: questionToUpdate.difficultyType,
                    hasLongAnswer: questionToUpdate.isLongAnswer,
                });
                toast.success("Question updated successfully!");
            }
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to save question");
        }
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
    const handlePoints = (questionId: string, value: string) => {
        const pointsValue = value === "" ? 0 : Math.max(0, parseInt(value, 10) || 0);
        setWrittenQuestions(prev => 
            prev.map(q => 
                q.id === questionId ? { ...q, points: pointsValue } : q
            )
        );
    };
    const handleSaveWrittenQuestions = async () => {
        try {
            const newQuestions = writtenQuestions.filter((q) => !q.questionId);
            const existingQuestions = writtenQuestions.filter((q) => q.questionId);
            if (newQuestions.length > 0) {
                const createResponse = await api.post("/Questions/Written/Create",{
                        examId: examId,
                        writtenQuestions: newQuestions.map((q) => ({
                            statementMarkdown: q.question,
                            points: q.points,
                            difficultyType: q.difficultyType,
                            hasLongAnswer: q.isLongAnswer,
                        })),
                    }
                );
                if (createResponse.status === 200) {
                        setWrittenQuestions((prev) =>
                        prev.map((q) => {
                            if (!q.questionId) {
                                const newQ = createResponse.data.find(
                                    // eslint-disable-next-line @typescript-eslint/no-explicit-any
                                    (newQuestion: any) =>
                                        newQuestion.statementMarkdown ===q.question &&
                                        newQuestion.points === q.points
                                );
                                return newQ
                                    ? { ...q, questionId: newQ.questionId }: q;
                            }
                            return q;
                        })
                    );
                }
            }
            const updatePromises = existingQuestions.map((q) =>
                api.patch("/Questions/Written/Update", {
                    questionId: q.questionId,
                    statementMarkdown: q.question,
                    points: q.points,
                    difficultyType: q.difficultyType,
                    hasLongAnswer: q.isLongAnswer,
                })
            );
            await Promise.all(updatePromises);
            toast.success("All questions saved successfully!");
            onSaved();
            setSaveButton(true);
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to save questions");
        }
    };
    const totalPages = useMemo(
        () => Math.ceil(writtenQuestions.length / questionsPerPage),
        [writtenQuestions, questionsPerPage]
    );
    const currentQuestions = useMemo(() =>
            writtenQuestions.slice(
                currentPage * questionsPerPage,
                (currentPage + 1) * questionsPerPage ),
        [writtenQuestions, currentPage, questionsPerPage]
    );
    return (
        <div>
            <Card
                className={`flex flex-col items-center shadow-none bg-white dark:bg-[#18181b]`}>
                <h2 className="text-2xl mt-3"> Written Question : {currentPage + 1} </h2>
                {currentQuestions.map((question) => (
                    <div key={question.id} className="w-full">
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
                                }/>
                            <div className="flex items-center gap-4 mt-5">
                                <Input
                                    className="w-32"
                                    type="number"
                                    label="Points"
                                    value={question.points.toString()} 
                                    onChange={(e) => {
                                        const value = e.target.value;
                                        if (value === "" || !isNaN(Number(value))) {
                                            handlePoints(question.id, value);
                                        }
                                    }}/>
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
                                <div className="w-full flex justify-end gap-3 mt-4">
                                {question.questionId && <Button color="primary" onPress={() =>
                                            handleUpdateQuestion(question.id)}>Update
                                    </Button>}
                                    <Button
                                        color="danger"
                                         onPress={() =>
                                            handleDeleteQuestion(
                                                question.id,
                                                question.questionId
                                            ) }>
                                        Delete
                                    </Button>
                                </div>
                            </div>
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
                </div>
            </Card>
            <div className="flex justify-center my-8 gap-3">
                <Button onPress={handleAddWrittenQuestion}>
                    Add Written Question
                </Button>
                <Button
                    isDisabled={saveButton}
                    color="primary"
                    onPress={handleSaveWrittenQuestions}
                >
                    Save All
                </Button>
            </div>
        </div>
    );
}
