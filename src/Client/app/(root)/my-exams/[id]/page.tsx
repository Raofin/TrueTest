"use client";

import React, { useEffect, useMemo, useState } from "react";
import { Button, Pagination } from "@heroui/react";
import CodeEditor from "@/components/submission/CodeEditor";
import "@/app/globals.css";
import getQuestionsForCurrentPage from "@/components/CurrPageQues";
import { FormatTimeHourMinutesSeconds } from "@/components/DateTimeFormat";
import WrittenSubmission from "@/components/submission/WrittenSubmition";
import MCQSubmission from "@/components/submission/McqSubmition";
import Logo from "@/components/ui/TrueTestLogo";
import { useParams, useRouter } from "next/navigation";
import api from "@/lib/api";
import toast from "react-hot-toast";
import LoadingModal from "@/components/ui/Modal/LoadingModal";
import { ProblemQuestion, TestCase, TestCaseResults } from '@/components/types/problemQues'
import { WrittenQuestion } from '@/components/types/writtenQues'
import {McqQuestion } from '@/components/types/mcqQues'
import { Exam, QuestionData } from '@/components/types/exam'

export default function Component() {
    const [currentPage, setCurrentPage] = useState(1);
    const { id } = useParams();
    const router = useRouter();
    const [timeLeft, setTimeLeft] = useState<number | undefined>();
    const [totalPage, setTotalPage] = useState(1);
    const [loading, setLoading] = useState(false);
    const [answers, setAnswers] = useState<{[key: string]: string | string[]}>({});
    const [regularQues, setRegularQues] = useState(0);
    const [codingQues, setCodingQues] = useState(0);
    const [isFullscreen, setIsFullscreen] = useState(false);
    const [questionsLeft, setQuestionsLeft] = useState(0);
    const [questions, setQuestions] = useState<QuestionData | null>(null);
    const [testCaseResults, setTestCaseResults] = useState<TestCaseResults>({});
    const [examActive, setExamActive] = useState(true);
    const allowedKeys = useMemo(() => new Set([
        'Backspace', 'Tab', 'Enter', 'Shift', 'CapsLock',
        'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown',
        'Home', 'End', 'Delete',
        'a','b','c','d','e','f','g','h','i','j','k','l','m',
        'n','o','p','q','r','s','t','u','v','w','x','y','z',
        '0','1','2','3','4','5','6','7','8','9',
        '(', ')', '{', '}', '[', ']', ';', ':', ',', '.', '=', '+', '-', '*', '/', '<', '>', '?', '!', '@', '#', '$', '%', '^', '&', '_', '|', '~', '`', '"', "'", '\\', ' '
      ]), []);
      useEffect(() => {
        const handleKeyDown = (e: KeyboardEvent) => {
          if (!examActive) return;
      
          const target = e.target as HTMLElement;
          const tag = target.tagName;
          const isInputOrTextarea = tag === 'INPUT' || tag === 'TEXTAREA';
          const isButton = tag === 'BUTTON';
          if (e.key === 'Escape') {
            e.preventDefault();
            e.stopPropagation();
            return;
          }
          if (isInputOrTextarea || isButton) return;
      
          let key = e.key;
          if (key.length === 1 && /^[A-Za-z]$/.test(key)) {
            key = key.toLowerCase();
          }
          const isBlockedCombination =
            ((e.ctrlKey || e.metaKey) && ['c', 'v', 'x'].includes(e.key.toLowerCase())) ||
            (e.key.startsWith('F') && +e.key.slice(1) >= 1 && +e.key.slice(1) <= 12) ||
            (!allowedKeys.has(key) && !e.ctrlKey && !e.metaKey && !e.altKey);
      
          if (isBlockedCombination) {
            e.preventDefault();
            e.stopPropagation();
          }
        };
        const handleClipboardEvent = (e: ClipboardEvent) => {
          if (!examActive) return;
          e.preventDefault();
          e.stopPropagation();
        };
      
        const handleContextMenu = (e: MouseEvent) => {
          if (!examActive) return;
          e.preventDefault();
          e.stopPropagation();
        };
      
        document.addEventListener('keydown', handleKeyDown);
        document.addEventListener('copy', handleClipboardEvent);
        document.addEventListener('paste', handleClipboardEvent);
        document.addEventListener('cut', handleClipboardEvent);
        document.addEventListener('contextmenu', handleContextMenu);
      
        return () => {
          document.removeEventListener('keydown', handleKeyDown);
          document.removeEventListener('copy', handleClipboardEvent);
          document.removeEventListener('paste', handleClipboardEvent);
          document.removeEventListener('cut', handleClipboardEvent);
          document.removeEventListener('contextmenu', handleContextMenu);
        };
      }, [examActive, allowedKeys]);
      
    useEffect(() => {
        const FetchExamData = async () => {
            setLoading(true);
            try {
                const resp = await api.get<Exam[]>(`/Candidate/Exams`);
                if (resp.status === 200 && resp.data) {
                    const selectedExam = resp.data.find(
                        (e) => e.exam.examId === id
                    );
                    if (selectedExam) {
                        let initialTimeLeft: number | undefined;
                        if (selectedExam.exam.closesAt) {
                            const now = new Date();
                            const closesAtDate = new Date(
                                selectedExam.exam.closesAt
                            );
                            const nowUTC = new Date(
                                now.getUTCFullYear(),
                                now.getUTCMonth(),
                                now.getUTCDate(),
                                now.getUTCHours(),
                                now.getUTCMinutes(),
                                now.getUTCSeconds()
                            );
                            const timeLeftInMs =
                                closesAtDate.getTime() - nowUTC.getTime();
                            const absTimeLeft = Math.abs(timeLeftInMs);
                            const timeLeftInSeconds = Math.floor(
                                absTimeLeft / 1000
                            );
                            initialTimeLeft =
                                selectedExam.exam.durationMinutes !== undefined
                                    ? Math.min(
                                          timeLeftInSeconds,
                                          selectedExam.exam.durationMinutes * 60
                                      )
                                    : timeLeftInSeconds;
                        } else if (
                            selectedExam.exam.durationMinutes !== undefined
                        ) {
                            initialTimeLeft =
                                selectedExam.exam.durationMinutes * 60;
                        }
                        setTimeLeft(() => initialTimeLeft);
                    } else {
                        toast.error("Exam not found");
                    }
                } else {
                    toast.error("Failed to fetch exams");
                }
            } catch (error) {
                console.error("Error fetching exam data:", error);
                toast.error("An unexpected error has occurred");
            } finally {
                setLoading(false);
            }
        };
        FetchExamData();
    }, [id]);

    useEffect(() => {
        const FetchQuestions = async () => {
            try {
                const resp = await api.post(`/Candidate/Exam/Start/${id}`);
                if (resp.status === 200) {
                    const normalizedData = {
                        questions: {
                            problem: resp.data.questions?.problem ?? [],
                            written: resp.data.questions?.written ?? [],
                            mcq: resp.data.questions?.mcq ?? [],
                        },
                        submits: resp.data.submits ?? {
                            problem: [],
                            written: [],
                            mcq: [],
                        },
                    };
                    setQuestions(normalizedData);
                    const initialAnswers: { [key: string]: string | string[] } =
                        {};
                    normalizedData.questions.problem.forEach(
                        (q: ProblemQuestion) => {
                            initialAnswers[q.questionId] = "";
                        }
                    );
                    normalizedData.questions.written.forEach(
                        (q: WrittenQuestion) => {
                            initialAnswers[q.questionId] = "";
                        }
                    );
                    normalizedData.questions.mcq.forEach((q: McqQuestion) => {
                        initialAnswers[q.questionId] = [];
                    });
                    setAnswers(initialAnswers);
                    const initialTestCaseResults: TestCaseResults = {};
                    normalizedData.questions.problem.forEach(
                        (q: ProblemQuestion) => {
                            initialTestCaseResults[q.questionId] =
                                q.testCases.map((tc: TestCase) => ({
                                    ...tc,
                                    receivedOutput: "",
                                    status: "pending",
                                }));
                        }
                    );
                    setTestCaseResults(initialTestCaseResults);
                }
            } catch {
                toast.error("An unexpected error has occurred");
            }
        };
        FetchQuestions();
    }, [id]);

    useEffect(() => {
        if (!questions) return;
        const regularQuestionsCount =
            questions.questions.mcq.length + questions.questions.written.length;
        const codingQuestionsCount = questions.questions.problem.length;
        setRegularQues(regularQuestionsCount);
        setCodingQues(codingQuestionsCount);
        setTotalPage(
            Math.ceil(
                Math.ceil(regularQuestionsCount / 5) + codingQuestionsCount
            )
        );
        setQuestionsLeft(regularQuestionsCount + codingQuestionsCount);
    }, [questions]);

    useEffect(() => {
        const timer = setInterval(() => {
          setTimeLeft((prev) => {
            if (prev !== undefined && prev <= 1) {
              setExamActive(false);
              clearInterval(timer);
              router.push("/my-exams");
              return 0;
            }
            return prev !== undefined ? prev - 1 : undefined;
          });
        }, 1000);
        return () => clearInterval(timer);
      }, [id, router, timeLeft]);
    const getOptionNumbers = (
        question: McqQuestion,
        selectedOptions: string[]
    ): string => {
        return selectedOptions
            .map((option) => {
                if (option === question.mcqOption.option1) return "1";
                if (option === question.mcqOption.option2) return "2";
                if (option === question.mcqOption.option3) return "3";
                if (option === question.mcqOption.option4) return "4";
                return "";
            })
            .filter(Boolean)
            .join(",");
    };

    const submitAllAnswers = async () => {
        setLoading(true);
        try {
            if (!questions) return;
            const mcqSubmissions = questions.questions.mcq
                .filter(
                    (q) =>
                        answers[q.questionId] &&
                        Array.isArray(answers[q.questionId])
                )
                .map((q) => ({
                    questionId: q.questionId,
                    candidateAnswerOptions: getOptionNumbers(
                        q,
                        answers[q.questionId] as string[]
                    ),
                }));
            const writtenSubmissions = questions.questions.written
                .filter(
                    (q) =>
                        answers[q.questionId] &&
                        typeof answers[q.questionId] === "string"
                )
                .map((q) => ({
                    questionId: q.questionId,
                    candidateAnswer: answers[q.questionId] as string,
                }));
            const promises = [];
            if (mcqSubmissions.length > 0) {
                promises.push(
                    api.put("/Candidate/Submit/Mcq/Save", {
                        examId: id,
                        submissions: mcqSubmissions,
                    })
                );
            }

            if (writtenSubmissions.length > 0) {
                promises.push(
                    api.put("/Candidate/Submit/Written/Save", {
                        examId: id,
                        submissions: writtenSubmissions,
                    })
                );
            }
            if (promises.length === 0) {
                toast.error("No answers to submit");
                return;
            }
            await Promise.all(promises);
            toast.success("All answers submitted successfully!");
            return true;
        } catch (error) {
            console.error("Error submitting answers:", error);
            toast.error("Failed to submit some answers");
            return false;
        } finally {
            setLoading(false);
        }
    };

    const handleSubmitAndExit = async () => {
        setExamActive(false);
        const success = await submitAllAnswers();
        if (success) {
          exitFullscreen();
          router.push("/my-exams");
        }
      };

    useEffect(() => {
        let count = 0;
        if (!questions) return;
        const allQuestions = [
            ...questions.questions.problem,
            ...questions.questions.written,
            ...questions.questions.mcq,
        ];
        allQuestions?.forEach((question) => {
            const answer = answers[question.questionId];
            if (answer) {
                if (Array.isArray(answer) && answer.length > 0) {
                    count++;
                } else if (typeof answer === "string" && answer.trim()) {
                    count++;
                }
            }
        });
        setQuestionsLeft(regularQues + codingQues - count);
    }, [answers, questions, regularQues, codingQues]);

    useEffect(() => {
        const handleFullscreenChange = () => {
            if (!document.fullscreenElement && !isFullscreen) {
                alert("You cannot exit fullscreen during the exam!");
                document.documentElement.requestFullscreen().catch((err) => {
                    console.error("Error re-entering fullscreen:", err);
                });
            }
        };
        document.addEventListener("fullscreenchange", handleFullscreenChange);
        return () => {
            document.removeEventListener(
                "fullscreenchange",
                handleFullscreenChange
            );
        };
    }, [isFullscreen]);

    const exitFullscreen = () => {
        setExamActive(false);
        if (document.exitFullscreen) {
            setIsFullscreen(true);
            document.exitFullscreen().catch((err) => {
                console.error("Error exiting fullscreen:", err);
            });
        }
    };

    useEffect(() => {
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen().catch((err) => {
                console.error("Error attempting fullscreen:", err);
            });
        }
    }, []);

    const currentQuestions = questions
        ? getQuestionsForCurrentPage({ currentPage, questions })
        : [];

    if (currentQuestions.length === 0) {
        return (
            <div className="flex justify-center items-center h-full">
                <p className="mt-16">Loading questions...</p>
            </div>
        );
    }

    const updateTestCaseResults = (
        questionId: string,
        newResults: TestCase[]
    ) => {
        setTestCaseResults((prevResults) => ({
            ...prevResults,
            [questionId]: newResults,
        }));
    };

    return (
        <>
            <LoadingModal isOpen={loading} message="Loading..." />
            <div className="py-3 mx-3">
                <div className="w-full px-2 flex items-center justify-between">
                    <div className="flex items-center gap-2">
                        <Logo />
                    </div>
                    <div className="flex gap-4 rounded-full border-small border-default-200/20 bg-background/60 px-4 py-2 shadow-medium backdrop-blur-md backdrop-saturate-150 dark:bg-default-100/50">
                        <div>
                            Questions Left: {questionsLeft}/
                            {codingQues + regularQues}
                        </div>
                        <div className="flex items-center gap-1 before:content-[''] before:w-2 before:h-2 before:bg-red-500 before:rounded-full">
                            <p>Time Left : </p>
                            <p
                                className={`font-mono ml-1 ${
                                    timeLeft !== undefined && timeLeft < 300
                                        ? "text-danger"
                                        : "text-success"
                                }`}
                            >
                                {timeLeft !== undefined
                                    ? FormatTimeHourMinutesSeconds({
                                          seconds: timeLeft,
                                      }) + " s"
                                    : "Loading..."}
                            </p>
                        </div>
                    </div>
                    <Button
                        onPress={handleSubmitAndExit}
                        color="primary"
                        size="md"
                        radius="full"
                    >
                        Submit Exam
                    </Button>
                </div>
            </div>

            <div className="mx-5 mt-3  border-none px-8 h-[90vh] flex flex-col justify-between">
                <div className={`space-y-8 rounded-lg `}>
                    {currentQuestions.map((question,index) => (
                        <div key={question.questionId} className="space-y-4">
                            {question.questionType === "MCQ" &&
                                (() => {
                                    const mcqQuestion = question as McqQuestion;
                                    const mcqOptions = [
                                        mcqQuestion.mcqOption?.option1,
                                        mcqQuestion.mcqOption?.option2,
                                        mcqQuestion.mcqOption?.option3,
                                        mcqQuestion.mcqOption?.option4,
                                    ].filter(Boolean) as string[];
                                    return (
                                        <>
                                            <div className="w-full flex justify-between">
                                                <h2 className="text-lg font-semibold">
                                                    #Question : {index}
                                                </h2>
                                                <p> points: {( question as McqQuestion).score }
                                                </p>
                                            </div>
                                            <MCQSubmission
                                                question={mcqQuestion}
                                                answers={answers}
                                                setAnswers={setAnswers}
                                                options={mcqOptions}
                                            />
                                        </>
                                    );
                                })()}
                            {question.questionType === "Written" && (
                                <>
                                    <div className="w-full flex justify-between">
                                        <h2 className="text-lg font-semibold">
                                            #Question: {index}
                                        </h2>
                                        <p>
                                            points:
                                            { (question as WrittenQuestion).score }
                                        </p>
                                    </div>
                                    <WrittenSubmission
                                        question={question as WrittenQuestion}
                                        setAnswers={setAnswers}
                                        answers={answers}
                                    />
                                </>
                            )}
                            {question.questionType === "ProblemSolving" && (
                                <>
                                    <div className="w-full flex justify-between">
                                        <h2 className="text-lg font-semibold">
                                            #Question: {index}
                                        </h2>
                                        <p>
                                            points:
                                            {(question as ProblemQuestion).points}
                                        </p>
                                    </div>
                                    <CodeEditor
                                        examId={id?.toString() ?? ""}
                                        question={question as ProblemQuestion}
                                        setAnswers={setAnswers}
                                        answers={answers}
                                        questionId={question.questionId}
                                        persistedTestCaseResults={
                                            testCaseResults[question.questionId]
                                        }
                                        onTestCaseRunComplete={
                                            updateTestCaseResults
                                        }
                                    />
                                </>
                            )}
                        </div>
                    ))}
                </div>
                <div className="flex justify-center items-end py-6">
                    <Pagination
                        total={totalPage}
                        page={currentPage}
                        onChange={setCurrentPage}
                        color="primary"
                        showControls
                        className="gap-2"
                    />
                </div>
            </div>
        </>
    );
}
