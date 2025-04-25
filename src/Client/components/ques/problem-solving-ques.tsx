"use client";

import React, { useState, useMemo, useEffect } from "react";
import { Form, Button, Textarea, Card, Input } from "@heroui/react";
import { Icon } from "@iconify/react";
import toast from "react-hot-toast";
import PaginationButtons from "@/components/ui/pagination-button";
import MdEditor from "../katex-mermaid";
import api from "@/lib/api";
import { AxiosError } from "axios";

interface TestCase {
    input: string;
    output: string;
}

interface Problem {
    questionId?: string;
    question: string;
    testCases: TestCase[];
    points: number;
    difficultyType?: string;
}

interface ProblemItemProps {
    problem: Problem;
    onDeleteTestCase: (testCaseIndex: number) => void;
    onRefreshTestCase: (testCaseIndex: number) => void;
    onTestCaseChange: (
        testCaseIndex: number,
        field: "input" | "output",
        value: string
    ) => void;
    onQuestionChange: (value: string) => void;
    onPointsChange: (value: number) => void;
    onAddTestCase: () => void;
    onDifficultyChange: (value: string) => void;
    onDeleteProblem: () => void;
    onUpdateProblem: () => void;
}

const ProblemItem: React.FC<ProblemItemProps> = ({
    problem,
    onDeleteTestCase,
    onRefreshTestCase,
    onTestCaseChange,
    onQuestionChange,
    onPointsChange,
    onDifficultyChange,
    onAddTestCase,
}) => (
    <div className="rounded-lg w-full flex flex-col justify-center items-center gap-4">
        <div className="w-full flex justify-end">
            <select
                className="rounded-md border p-2 dark:bg-[#1e293b] dark:text-gray-300"
                value={problem.difficultyType}
                onChange={(e) => onDifficultyChange(e.target.value)}
            >
                <option value="">Select difficulty</option>
                <option value="easy">Easy</option>
                <option value="medium">Medium</option>
                <option value="hard">Hard</option>
            </select>
        </div>
        <div className="dark:bg-[#18181b] dark:text-white w-full">
            <MdEditor value={problem.question} onChange={onQuestionChange} />
        </div>
        {problem.testCases.map((testCase, index) => (
            <TestCaseItem
                key={index}
                testCaseIndex={index}
                testCase={testCase}
                onDelete={onDeleteTestCase}
                onRefresh={onRefreshTestCase}
                onInputChange={(field, value) =>
                    onTestCaseChange(index, field, value)
                }
            />
        ))}
        <div className="w-full flex justify-between">
            <div>
                <Input
                    className="w-32"
                    type="number"
                    label="Points"
                    value={problem.points.toString()}
                    onChange={(e) => onPointsChange(parseInt(e.target.value))}
                />
            </div>
            <div>
                <Button
                    variant="flat"
                    startContent={<Icon icon="lucide:plus" />}
                    onPress={onAddTestCase}
                    className="w-full"
                >
                    Add Test Case
                </Button>
            </div>
        </div>
    </div>
);

interface TestCaseItemProps {
    testCaseIndex: number;
    testCase: TestCase;
    onDelete: (testCaseIndex: number) => void;
    onRefresh: (testCaseIndex: number) => void;
    onInputChange: (field: "input" | "output", value: string) => void;
}

const TestCaseItem: React.FC<TestCaseItemProps> = ({
    testCaseIndex,
    testCase,
    onDelete,
    onRefresh,
    onInputChange,
}) => (
    <div className="w-full flex gap-2">
        <div className="flex flex-col gap-2">
            <Button
                isIconOnly
                variant="flat"
                onPress={() => onDelete(testCaseIndex)}
            >
                <Icon icon="lucide:trash-2" width={20} />
            </Button>
            <Button
                isIconOnly
                variant="flat"
                onPress={() => onRefresh(testCaseIndex)}
            >
                <Icon icon="lucide:refresh-cw" width={20} />
            </Button>
        </div>
        <div className="w-full flex gap-3">
            <Textarea
                label="Input"
                value={testCase.input}
                className="flex-1 rounded-2xl"
                onChange={(e) => onInputChange("input", e.target.value)}
            />
            <Textarea
                label="Expected Output"
                value={testCase.output}
                className="flex-1 rounded-2xl"
                onChange={(e) => onInputChange("output", e.target.value)}
            />
        </div>
    </div>
);

interface FormFooterProps {
    totalPages: number;
    currentPage: number;
    onPrevious: () => void;
    onNext: () => void;
}

const FormFooter: React.FC<FormFooterProps> = ({
    totalPages,
    currentPage,
    onPrevious,
    onNext,
}) => (
        <div className="flex gap-2 items-center">
            <span>
                Page {currentPage + 1} of {totalPages}
            </span>
            <PaginationButtons
                currentIndex={currentPage + 1}
                totalItems={totalPages}
                onPrevious={onPrevious}
                onNext={onNext}
            />
        </div>
);

interface ProblemSolvingFormProps {
    readonly examId: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    readonly existingQuestions: any[];
    readonly onSaved: () => void;
    readonly problemPoints: (points: number) => void;
}

export default function ProblemSolvingForm({
    examId,
    existingQuestions,
    onSaved,
    problemPoints,
}: ProblemSolvingFormProps) {
    const [problems, setProblems] = useState<Problem[]>(
        existingQuestions.length > 0
            ? existingQuestions.map((q) => ({
                  questionId: q.questionId,
                  question: q.statementMarkdown,
                  testCases: q.testCases ?? [{ input: "", output: "" }],
                  points: q.points,
                  difficultyType: q.difficultyType,
              }))
            : [
                  {
                      question: "",
                      testCases: [{ input: "", output: "" }],
                      points: 0,
                      difficultyType: "",
                  },
              ]
    );
    const [currentPage, setCurrentPage] = useState(0);
    const problemsPerPage = 1;
    const handleDeleteProblem = async (problemIndex: number) => {
       const problemToDelete = problems[problemIndex];
        try {
            if (problemToDelete.questionId) {
                await api.delete(`/Questions/Problem/Delete/${problemToDelete.questionId}`);
            }
            if(problems.length==1){
                setProblems([{
                    question: "",
                    testCases: [{ input: "", output: "" }],
                    points: 0,
                    difficultyType: "",
                }])
            }else {
            setProblems((prev) => prev.filter((_, i) => i !== problemIndex));
            if (currentPage >=Math.ceil((problems.length - 1) / problemsPerPage)) {
                setCurrentPage(Math.max(0, currentPage - 1));
            }
            }
            toast.success("Problem deleted successfully");
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to delete problem");
        }
    };
    useEffect(() => {
        const total = problems.reduce(
            (sum, problem) => sum + (problem.points || 0),
            0
        );
        problemPoints(total);
    }, [problems, problemPoints]);

    const handleSaveProblem = async (e: React.FormEvent) => {
        e.preventDefault();
        const hasMissingDifficulty = problems.some(
            (problem) =>
                !problem.difficultyType || problem.difficultyType.trim() === ""
        );
        const hasMissingPoints = problems.some((problem) => !problem.points);

        if (hasMissingDifficulty) {
            toast.error("Please select a difficulty level");
            return;
        }
        if (hasMissingPoints) {
            toast.error("Please input points of the problem");
            return;
        }
        try {
            const newProblems = problems.filter((p) => !p.questionId);
            const existingProblems = problems.filter((p) => p.questionId);
            if (newProblems.length > 0) {
                const createResponse = await api.post(
                    "/Questions/Problem/Create",
                    {
                        examId,
                        problemQuestions: newProblems.map((problem) => ({
                            statementMarkdown: problem.question,
                            points: problem.points,
                            difficultyType: problem.difficultyType,
                            testCases: problem.testCases,
                        })),
                    }
                );

                if (createResponse.status === 200) {
                    setProblems((prev) =>
                        prev.map((p) => {
                            if (!p.questionId) {
                                const newProblem = createResponse.data.find(
                                    // eslint-disable-next-line @typescript-eslint/no-explicit-any
                                    (np: any) =>
                                        np.statementMarkdown === p.question &&
                                        np.points === p.points
                                );
                                return newProblem
                                    ? {
                                          ...p,
                                          questionId: newProblem.questionId,
                                      }
                                    : p;
                            }
                            return p;
                        })
                    );
                }
            }
            const updatePromises = existingProblems.map((problem) => {
                if (!problem.questionId) return;

                return api.patch("/Questions/Problem/Update", {
                    questionId: problem.questionId,
                    statementMarkdown: problem.question,
                    points: problem.points,
                    difficultyType: problem.difficultyType,
                    testCases: problem.testCases,
                });
            });

            await Promise.all(updatePromises);
            toast.success("Problems saved successfully!");
            onSaved();
            setSaveButton(!saveButton);
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to save problems");
        }
    };
    const handleUpdateProblem = async (problemIndex: number) => {
        const problemToUpdate = problems[problemIndex];
        if (!problemToUpdate) return;
        try {
            if (!problemToUpdate.questionId) {
                const response = await api.post("/Questions/Problem/Create", {
                    examId,
                    problemQuestions: [
                        {
                            statementMarkdown: problemToUpdate.question,
                            points: problemToUpdate.points,
                            difficultyType: problemToUpdate.difficultyType,
                            testCases: problemToUpdate.testCases,
                        },
                    ],
                });

                if (response.status === 200) {
                    setProblems((prev) =>
                        prev.map((p, i) =>
                            i === problemIndex
                                ? {
                                      ...p,
                                      questionId: response.data[0]?.questionId,
                                  }
                                : p
                        )
                    );
                    toast.success("Problem saved successfully!");
                }
            } else {
                await api.patch("/Questions/Problem/Update", {
                    questionId: problemToUpdate.questionId,
                    statementMarkdown: problemToUpdate.question,
                    points: problemToUpdate.points,
                    difficultyType: problemToUpdate.difficultyType,
                    testCases: problemToUpdate.testCases,
                });
                toast.success("Problem updated successfully!");
            }
        } catch (error) {
            const err = error as AxiosError;
            toast.error(err.message ?? "Failed to save problem");
        }
    };
    const updateProblem = (index: number, updatedProblem: Problem) => {
        setProblems((prev) => [
            ...prev.slice(0, index),
            updatedProblem,
            ...prev.slice(index + 1),
        ]);
    };

    const handleQuestionChange = (index: number, newQuestion: string) => {
        updateProblem(index, {
            ...problems[index],
            question: newQuestion,
        });
    };

    const handlePointsChange = (index: number, newPoints: number) => {
        updateProblem(index, {
            ...problems[index],
            points: newPoints,
        });
    };

    const addTestCase = (problemIndex: number) => {
        const updatedProblem = {
            ...problems[problemIndex],
            testCases: [
                ...problems[problemIndex].testCases,
                { input: "", output: "" },
            ],
        };
        updateProblem(problemIndex, updatedProblem);
    };

    const handleDeleteTestCase = (
        problemIndex: number,
        testCaseIndex: number
    ) => {
        const updatedProblem = {
            ...problems[problemIndex],
            testCases: problems[problemIndex].testCases.filter(
                (_, i) => i !== testCaseIndex
            ),
        };

        if (updatedProblem.testCases.length === 0) {
            updatedProblem.testCases = [{ input: "", output: "" }];
        }

        updateProblem(problemIndex, updatedProblem);
    };

    const handleRefreshTestCase = (
        problemIndex: number,
        testCaseIndex: number
    ) => {
        const updatedProblem = {
            ...problems[problemIndex],
            testCases: problems[problemIndex].testCases.map((tc, i) =>
                i === testCaseIndex ? { input: "", output: "" } : tc
            ),
        };
        updateProblem(problemIndex, updatedProblem);
        toast.success("Test case refreshed");
    };

    const handleTestCaseChange = (
        problemIndex: number,
        testCaseIndex: number,
        field: "input" | "output",
        value: string
    ) => {
        const updatedProblem = {
            ...problems[problemIndex],
            testCases: problems[problemIndex].testCases.map((tc, i) =>
                i === testCaseIndex ? { ...tc, [field]: value } : tc
            ),
        };
        updateProblem(problemIndex, updatedProblem);
    };

    const handleAddProblem = () => {
        setProblems((prev) => [
            ...prev,
            {
                question: "",
                testCases: [{ input: "", output: "" }],
                points: 0,
                difficultyType: "",
            },
        ]);
        setCurrentPage(Math.ceil((problems.length + 1) / problemsPerPage) - 1);
    };

    const handleDifficultyChange = (index: number, newDifficulty: string) => {
        updateProblem(index, {
            ...problems[index],
            difficultyType: newDifficulty,
        });
    };
    const totalPages = useMemo(
        () => Math.ceil(problems.length / problemsPerPage),
        [problems, problemsPerPage]
    );
    const currentProblems = useMemo(
        () =>
            problems.slice(
                currentPage * problemsPerPage,
                (currentPage + 1) * problemsPerPage
            ),
        [problems, currentPage, problemsPerPage]
    );
    const currentProblemIndex = currentPage * problemsPerPage;
    const [saveButton, setSaveButton] = useState(false);
    return (
        <div>
            <Form
                className="w-full flex flex-col gap-4 p-5 border-none"
                onSubmit={handleSaveProblem}
            >
                <Card className="w-full border-none shadow-none bg-white dark:bg-[#18181b]">
                    <h2 className="w-full flex justify-center text-2xl my-3">
                        Problem Solving Question : {currentProblemIndex + 1}
                    </h2>
                    <div className="w-full flex flex-col justify-center p-4">
                        {currentProblems.map((problem, index) => (
                            <div key={index}>
                                <ProblemItem
                                    problem={{
                                        ...problem,
                                        points: problem.points,
                                    }}
                                    onDeleteTestCase={(testCaseIndex) =>
                                        handleDeleteTestCase(
                                            currentProblemIndex + index,
                                            testCaseIndex
                                        )
                                    }
                                    onRefreshTestCase={(testCaseIndex) =>
                                        handleRefreshTestCase(
                                            currentProblemIndex + index,
                                            testCaseIndex
                                        )
                                    }
                                    onTestCaseChange={(
                                        testCaseIndex,
                                        field,
                                        value
                                    ) =>
                                        handleTestCaseChange(
                                            currentProblemIndex + index,
                                            testCaseIndex,
                                            field,
                                            value
                                        )
                                    }
                                    onQuestionChange={(value) =>
                                        handleQuestionChange(
                                            currentProblemIndex + index,
                                            value
                                        )
                                    }
                                    onPointsChange={(value) =>
                                        handlePointsChange(
                                            currentProblemIndex + index,
                                            value
                                        )
                                    }
                                    onDifficultyChange={(value) =>
                                        handleDifficultyChange(
                                            currentProblemIndex + index,
                                            value
                                        )
                                    }
                                    onAddTestCase={() =>
                                        addTestCase(currentProblemIndex + index)
                                    }
                                    onDeleteProblem={() =>
                                        handleDeleteProblem(
                                            currentProblemIndex + index
                                        )
                                    }
                                    onUpdateProblem={() =>
                                        handleUpdateProblem(
                                            currentProblemIndex + index
                                        )
                                    }
                                />
                                <div className="flex w-full justify-between mt-5">
                                <div />
                                    <FormFooter
                                        totalPages={totalPages}
                                        currentPage={currentPage}
                                        onPrevious={() =>
                                            setCurrentPage(
                                                Math.max(0, currentPage - 1)
                                            )
                                        }
                                        onNext={() =>
                                            setCurrentPage(
                                                Math.min(
                                                    totalPages - 1,
                                                    currentPage + 1
                                                )
                                            )
                                        }
                                    />
                                    <div className="flex  gap-3">
                                        {problem.questionId && (
                                            <Button
                                                color="primary"
                                                onPress={() =>handleUpdateProblem(currentProblemIndex +index)}>
                                                Update
                                            </Button>
                                        )}
                                        <Button
                                            color="danger"
                                            onPress={() =>
                                                handleDeleteProblem(
                                                    currentProblemIndex + index
                                                )}>
                                            Delete
                                        </Button>
                                    </div>
                                </div>
                            </div> ))}
                    </div>
                </Card>
                <div className="flex gap-3 w-full justify-center mt-5">
                    <Button onPress={handleAddProblem}>
                        Add Problem Solving Question
                    </Button>
                    <Button
                        type="submit"
                        color="primary"
                        isDisabled={saveButton}>
                        Save All
                    </Button>
                </div>
            </Form>
        </div>
    );
}
