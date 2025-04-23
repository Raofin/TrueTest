"use client";

import React, { useEffect, useState } from "react";
import {
    Button,
    Textarea,
    Card,
    CardBody,
    CardHeader,
    Checkbox,
    Input,
} from "@heroui/react";
import PaginationButtons from "@/components/ui/pagination-button";
import api from "@/lib/api";
import toast from "react-hot-toast";
import { AxiosError } from "axios";

interface MCQOption {
    id: number;
    text: string;
}

interface MCQQuestion {
    questionId?: string;
    question: string;
    points: number;
    options: MCQOption[];
    correctOptions: number[];
    difficultyType: string;
}
interface ExistingQuestion {
    questionId?: string;
    statementMarkdown: string;
    points: number;
    difficultyType: string;
    mcqOption: {
        option1: string;
        option2: string;
        option3: string;
        option4: string;
        isMultiSelect: boolean;
        answerOptions: string;
    };
}

interface MCQFormProps {
    readonly examId: string;
    readonly existingQuestions: ExistingQuestion[];
    readonly onSaved: () => void;
    readonly mcqPoints: (points: number) => void;
}
export default function App({
    examId,
    existingQuestions,
    onSaved,
    mcqPoints,
}: MCQFormProps) {
    const [questions, setQuestions] = useState<MCQQuestion[]>(
        existingQuestions.length > 0
            ? existingQuestions.map((q) => ({
                  questionId: q.questionId,
                  question: q.statementMarkdown,
                  points: Number(q.points) || 0,
                  difficultyType: "Easy",
                  options: [
                      { id: 1, text: q.mcqOption.option1 },
                      { id: 2, text: q.mcqOption.option2 },
                      { id: 3, text: q.mcqOption.option3 },
                      { id: 4, text: q.mcqOption.option4 },
                  ],
                  correctOptions: q.mcqOption.answerOptions
                      .split(",")
                      .map(Number),
              }))
            : [
                  {
                      questionId: "",
                      question: "",
                      options: [
                          { id: 1, text: "" },
                          { id: 2, text: "" },
                          { id: 3, text: "" },
                          { id: 4, text: "" },
                      ],
                      correctOptions: [],
                      points: 0,
                      difficultyType: "Easy",
                  },
              ]
    );
    const handleDeleteQuestion = async (index: number) => {
        const questionToDelete = questions[index];

        try {
            if (questionToDelete.questionId) 
                await api.delete(`/Questions/Mcq/Delete/${questionToDelete.questionId}`);
            setQuestions((prev) => prev.filter((_, i) => i !== index));
            if (currentPage >= questions.length - 1) {
                setCurrentPage(Math.max(0, currentPage - 1));
            }
            toast.success("Question deleted successfully");
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to delete question");
        }
    };
    const [currentPage, setCurrentPage] = useState(0);
    const [saveButton, setSaveButton] = useState(false);
    const handleQuestionChange = (index: number, value: string) => {
        const newQuestions = [...questions];
        newQuestions[index].question = value;
        setQuestions(newQuestions);
    };
    useEffect(() => {
        const total = questions.reduce(
            (sum, problem) => sum + (problem.points || 0),
            0
        );
        mcqPoints(total);
    }, [questions, mcqPoints]);
    const handleOptionChange = (
        questionIndex: number,
        optionId: number,
        value: string
    ) => {
        const newQuestions = [...questions];
        const optionIndex = newQuestions[questionIndex].options.findIndex(
            (opt) => opt.id === optionId
        );
        newQuestions[questionIndex].options[optionIndex].text = value;
        setQuestions(newQuestions);
    };

    const handleCorrectOptionChange = (
        questionIndex: number,
        optionId: number
    ) => {
        const newQuestions = [...questions];
        const currentQuestion = newQuestions[questionIndex];
        const currentCorrectOptions = currentQuestion.correctOptions;
        const optionIndex = currentCorrectOptions.indexOf(optionId);
        if (optionIndex === -1) {
            currentCorrectOptions.push(optionId);
        } else {
            currentCorrectOptions.splice(optionIndex, 1);
        }

        setQuestions(newQuestions);
    };

    const addNewQuestion = () => {
        setQuestions([
            ...questions,
            {
                question: "",
                options: [
                    { id: 1, text: "" },
                    { id: 2, text: "" },
                    { id: 3, text: "" },
                    { id: 4, text: "" },
                ],
                correctOptions: [],
                points: 0,
                difficultyType: "Easy",
            },
        ]);
        setCurrentPage(questions.length);
    };
    const handleUpdateQuestion = async (index: number) => {
        const questionToUpdate = questions[index];
        if (!questionToUpdate) return;

        try {
            if (!questionToUpdate.questionId) {
                const response = await api.post("/Questions/Mcq/Create", {
                    examId: examId,
                    mcqQuestions: [
                        {
                            statementMarkdown: questionToUpdate.question,
                            points: questionToUpdate.points,
                            difficultyType: "Easy",
                            mcqOption: {
                                option1: questionToUpdate.options[0].text,
                                option2: questionToUpdate.options[1].text,
                                option3: questionToUpdate.options[2].text,
                                option4: questionToUpdate.options[3].text,
                                isMultiSelect:
                                    questionToUpdate.correctOptions.length > 1,
                                answerOptions:
                                    questionToUpdate.correctOptions.join(","),
                            },
                        },
                    ],
                });

                if (response.status === 200) {
                    setQuestions((prev) => prev.map((q, i) =>i === index ? {
                                      ...q, questionId: response.data[0]?.questionId,
                                  }: q
                        ));
                    toast.success("Question saved successfully!");
                }
            } else {
                await api.patch("/Questions/Mcq/Update", {
                    questionId: questionToUpdate.questionId,
                    statementMarkdown: questionToUpdate.question,
                    points: questionToUpdate.points,
                    difficultyType: questionToUpdate.difficultyType,
                    mcqOption: {
                        option1: questionToUpdate.options[0].text,
                        option2: questionToUpdate.options[1].text,
                        option3: questionToUpdate.options[2].text,
                        option4: questionToUpdate.options[3].text,
                        isMultiSelect:
                            questionToUpdate.correctOptions.length > 1,
                        answerOptions:
                            questionToUpdate.correctOptions.join(","),
                    },
                });
                toast.success("Question updated successfully!");
            }
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to save question");
        }    };
    const handlePoints = (questionIndex: number, value: string) => {
        const newQuestions = [...questions];
        newQuestions[questionIndex].points =
            value === "" ? 0 : Math.max(0, parseInt(value) || 0);
        setQuestions(newQuestions);
    };
    const handleSubmit = async () => {
        const hasMissingPoints = questions.some((problem) => !problem.points);
        if (hasMissingPoints) {
            toast.error("Please input points of the mcq question");
            return;
        }
        try {
            const newQuestions = questions.filter((q) => !q.questionId);
            const existingQuestions = questions.filter((q) => q.questionId);
            if (newQuestions.length > 0) {
                const createResponse = await api.post("/Questions/Mcq/Create", {
                    examId: examId,
                    mcqQuestions: newQuestions.map((q) => ({
                        statementMarkdown: q.question,
                        points: q.points,
                        difficultyType: "Easy",
                        mcqOption: {
                            option1: q.options[0].text,
                            option2: q.options[1].text,
                            option3: q.options[2].text,
                            option4: q.options[3].text,
                            isMultiSelect: q.correctOptions.length > 1,
                            answerOptions: q.correctOptions.join(","),
                        },
                    })),
                });

                if (createResponse.status === 200) {
                    setQuestions((prev) =>
                        prev.map((q) => {
                            if (!q.questionId) {
                                const newQ = createResponse.data.find(
                                    // eslint-disable-next-line @typescript-eslint/no-explicit-any
                                    (newQuestion: any) =>
                                        newQuestion.statementMarkdown ===
                                            q.question &&
                                        newQuestion.points === q.points
                                );
                                return newQ
                                    ? { ...q, questionId: newQ.questionId }
                                    : q;
                            }
                            return q;
                        })
                    );
                }
            }
            const updatePromises = existingQuestions.map((q) => {
                if (!q.questionId) return;

                return api.patch("/Questions/Mcq/Update", {
                    questionId: q.questionId,
                    statementMarkdown: q.question,
                    points: q.points,
                    difficultyType: q.difficultyType,
                    mcqOption: {
                        option1: q.options[0].text,
                        option2: q.options[1].text,
                        option3: q.options[2].text,
                        option4: q.options[3].text,
                        isMultiSelect: q.correctOptions.length > 1,
                        answerOptions: q.correctOptions.join(","),
                    },
                });
            });

            await Promise.all(updatePromises);
            toast.success("MCQ questions saved successfully!");
            onSaved();
            setSaveButton(!saveButton);
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to save questions");
        }
    };
    return (
        <div className="flex justify-center ">
            <div className="w-full flex flex-col">
                {questions.length > 0 && (<>
                    <Card
                        key={currentPage}
                        className="w-full shadow-none bg-white dark:bg-[#18181b]"
                    >
                        <CardHeader className="flex flex-col gap-2 ">
                            <h2 className="text-2xl my-3">
                                MCQ Question : {currentPage + 1}
                            </h2>
                        </CardHeader>
                        <CardBody className="flex flex-col gap-4 p-8">
                            <Textarea
                                label="mcq question"
                                minRows={5}
                                value={questions[currentPage].question}
                                className="bg-[#eeeef0] dark:[#71717a] rounded-2xl"
                                onChange={(e: { target: { value: string } }) =>
                                    handleQuestionChange(
                                        currentPage,
                                        e.target.value
                                    )
                                }
                            />
                            <div className="grid grid-cols-2 gap-4">
                                {questions[currentPage].options.map(
                                    (option) => (
                                        <div
                                            key={option.id}
                                            className="flex items-center gap-2"
                                        >
                                            <Checkbox
                                                isSelected={questions[
                                                    currentPage
                                                ].correctOptions.includes(
                                                    option.id
                                                )}
                                                onChange={() =>
                                                    handleCorrectOptionChange(
                                                        currentPage,
                                                        option.id
                                                    )
                                                }
                                                name={`option-${option.id}`}
                                                size="sm"
                                            />
                                            <Textarea
                                                className="bg-[#eeeef0] dark:[#71717a] rounded-2xl flex-grow"
                                                label={`Option ${option.id}`}
                                                value={option.text}
                                                isRequired={
                                                    option.id === 1 ||
                                                    option.id === 2
                                                }
                                                onChange={(e: {
                                                    target: { value: string };
                                                }) =>
                                                    handleOptionChange(
                                                        currentPage,
                                                        option.id,
                                                        e.target.value
                                                    )
                                                }
                                            />
                                        </div>
                                    )
                                )}
                            </div>
                        </CardBody>
                        <div className="w-full flex justify-between px-8 py-5">
                            <Input
                                className="w-32"
                                type="number"
                                label="Points"
                                value={questions[currentPage].points.toString() ??"0"}
                                onChange={(e) => handlePoints(currentPage, e.target.value) }
                                min="0"
                            />
                            <div className="flex items-center gap-2 ">
                                <span> Page {currentPage + 1} of {questions.length}</span>
                                <PaginationButtons
                                    currentIndex={currentPage + 1}
                                    totalItems={questions.length}
                                    onPrevious={() =>
                                        setCurrentPage((prev) =>
                                            Math.max(prev - 1, 0)
                                        )
                                    }
                                    onNext={() =>
                                        setCurrentPage((prev) =>
                                            Math.min(
                                                prev + 1,
                                                questions.length - 1
                                            )
                                        )
                                    }
                                />
                            </div>
                            <div className="w-full flex justify-end gap-3">
                            {questions[currentPage].questionId && <Button color="primary"
                                    onPress={() => handleUpdateQuestion(currentPage)}>
                                        Update</Button>}
                                <Button color="danger"
                                    onPress={() =>handleDeleteQuestion(currentPage)}
                                    className="mb-4">
                                    Delete </Button>
                            </div>
                        </div>
                    </Card>
                <div className="w-full flex gap-3 my-3 text-center justify-center">
                    <Button onPress={addNewQuestion}>Add MCQ Question</Button>
                    <Button
                        color="primary"
                        isDisabled={saveButton}
                        onPress={handleSubmit}>
                        Save All
                    </Button>
                </div></>
                )}
            </div>
        </div>
    );
}
