"use client";


import React, { useEffect, useState } from "react";
import { Button, Link, Pagination } from "@heroui/react";
import CodeEditor from "@/components/submission/code-editor";
import "@/app/globals.css";
import getQuestionsForCurrentPage from "@/components/currpage-ques";
import { FormatTimeHourMinutesSeconds } from "@/components/format-date-time";
import StartExam from "@/components/start-exam";
import WrittenSubmission from "@/components/submission/written-submit";
import MCQSubmission from "@/components/submission/mcq-submit";
import Logo from "@/components/ui/logo/page";
import { useParams } from "next/navigation";
import api from "@/lib/api";
import toast from "react-hot-toast";


export interface TestCase {
    testCaseId?: string;
    input: string;
    output: string;
    receivedOutput?: string;
    status?: "success" | "error" | "pending";
}


export interface ProblemQuestion {
    questionId: string;
    examId: string;
    questionType: string;
    statementMarkdown: string;
    points: number;
    difficultyType: string;
    testCases: TestCase[];
}


interface WrittenQuestion {
    questionId: string;
    examId: string;
    questionType: string;
    hasLongAnswer: boolean;
    statementMarkdown: string;
    score: number;
    difficultyType: string;
}


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


interface Questions {
    problem: ProblemQuestion[];
    written: WrittenQuestion[];
    mcq: MCQQuestion[];
}


interface QuestionData {
    questions: Questions;
    submits: {
        problem: [];
        written: [];
        mcq: [];
    };
}
interface ExamData {
    examId: string;
    title: string;
    totalPoints: number;
    durationMinutes: string;
    problemSolvingPoints: number;
    status: string;
    writtenPoints: number;
    mcqPoints: number;
    opensAt: string;
    closesAt: string;
}
export default function Component() {
    const [currentPage, setCurrentPage] = useState(1);
    const [timeLeft, setTimeLeft] = useState(24*3600*3600);
    const [examStarted, setExamStarted] = useState(false);
    const [totalPage, setTotalPage] = useState(1);
    const [answers, setAnswers] = useState<{
        [key: string]: string | string[];
    }>({});
    const [regularQues, setRegularQues] = useState(0);
    const [codingQues, setCodingQues] = useState(0);
    const [isFullscreen, setIsFullscreen] = useState(false);
    const [questionsLeft, setQuestionsLeft] = useState(0);
    const [startExamPage, setStartExamPage] = useState<ExamData>();
    const [questions, setQuestions] = useState<QuestionData | null>(null);
    const { id } = useParams();
    useEffect(() => {
        const FetchData = async () => {
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
                    console.log(normalizedData);
                }
            } catch {
                toast.error("An unexpected error has occured");
            }
        };
        FetchData();
    }, [id]);
    useEffect(() => {
        if (!questions) return;
        const regularQuestionsCount =
            questions.questions.mcq.length + questions.questions.written.length;
        const codingQuestionsCount = questions.questions.problem.length;
        setRegularQues(regularQuestionsCount);
        setCodingQues(codingQuestionsCount);
        setTotalPage(Math.ceil((Math.ceil(regularQuestionsCount/5)+ codingQuestionsCount)));
        setQuestionsLeft(regularQuestionsCount + codingQuestionsCount);
    }, [questions]);


    useEffect(() => {
        if (startExamPage && examStarted) {
            const examDurationSeconds = parseInt(startExamPage.durationMinutes) * 60;
           console.log(examDurationSeconds,timeLeft)
            setTimeLeft(Math.min(examDurationSeconds,timeLeft));
            const timer = setInterval(() => {
                setTimeLeft((prev) => {
                    if (prev <= 1) {
                        clearInterval(timer);
                        return 0;
                    }
                    return prev - 1;
                });
            }, 1000);
            return () => clearInterval(timer);
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [examStarted, startExamPage]);
    const getOptionNumbers = (question: MCQQuestion, selectedOptions: string[]): string => {
        return selectedOptions
          .map(option => {
            if (option === question.mcqOption.option1) return '1';
            if (option === question.mcqOption.option2) return '2';
            if (option === question.mcqOption.option3) return '3';
            if (option === question.mcqOption.option4) return '4';
            return '';
          })
          .filter(Boolean)
          .join(',');
      };
      
      const submitAllAnswers = async () => {
        try {
          if (!questions) return;
          const mcqSubmissions = questions.questions.mcq
            .filter(q => answers[q.questionId] && Array.isArray(answers[q.questionId]))
            .map(q => ({
              questionId: q.questionId,
              candidateAnswerOptions: getOptionNumbers(q, answers[q.questionId] as string[]),
            }));
          const writtenSubmissions = questions.questions.written
            .filter(q => answers[q.questionId] && typeof answers[q.questionId] === 'string')
            .map(q => ({
              questionId: q.questionId,
              candidateAnswer: answers[q.questionId] as string,
            }));
      
          const promises = [];
          
          if (mcqSubmissions.length > 0) {
            promises.push(api.put('/Candidate/Submit/Mcq/Save', {
              examId: id,
              submissions: mcqSubmissions
            }));
          }
      
          if (writtenSubmissions.length > 0) {
            promises.push(api.put('/Candidate/Submit/Written/Save', {
              examId: id,
              submissions: writtenSubmissions
            }));
          }
      
          if (promises.length === 0) {
            toast.error('No answers to submit');
            return;
          }
      
          await Promise.all(promises);
          toast.success('All answers submitted successfully!');
          return true;
        } catch (error) {
          console.error('Error submitting answers:', error);
          toast.error('Failed to submit some answers');
          return false;
        }
      };
      
      
      const handleSubmitAndExit = async () => {
        const success = await submitAllAnswers();
        if (success) {
          exitFullscreen();
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
                if (Array.isArray(answer)) {
                    if (answer.length > 0) {
                        count++;
                    }
                } else if (typeof answer === "string") {
                    if (answer.trim()) {
                        count++;
                    }
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


    useEffect(() => {
        const FetchData = async () => {
            try {
                const resp = await api.get(`/Candidate/Exams`);
                if (resp.status === 200) {
                    const exam = resp.data.find(
                        (e: ExamData) => e.examId === id
                    );
                    setStartExamPage(exam);
                }
            } catch {}
        };
        FetchData();
    }, [id]);
    const exitFullscreen = () => {
        if (document.exitFullscreen) {
            setIsFullscreen(true);
            document.exitFullscreen().catch((err) => {
                console.error("Error exiting fullscreen:", err);
            });
        }
    };
    const startExam = (initialTime: number) => {
        setExamStarted(true);
        setTimeLeft(initialTime);
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen().catch((err) => {
                console.error("Error attempting fullscreen:", err);
            });
        }
    };
    if (startExamPage) {
        return (
            <StartExam
                startExamPage={startExamPage}
                setExamStarted={setExamStarted}
                startExam={startExam}
            />
        );
    }
    const currentQuestions = questions
        ? getQuestionsForCurrentPage({
              currentPage,
              questions,
          })
        : [];
    console.log(currentPage, currentQuestions);
    if (currentQuestions.length === 0) {
        return (
            <div className="flex justify-center items-center h-full">
                <p className="mt-16">Loading questions...</p>
            </div>
        );
    }
    return (
        <>
            {examStarted && (
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
                                        timeLeft < 300
                                            ? "text-danger"
                                            : "text-success"
                                    }`}
                                >
                                    {FormatTimeHourMinutesSeconds({
                                        seconds: timeLeft,
                                    })}
                                    s
                                </p>
                            </div>
                        </div>
                        <Link href="/my-exams">
                            <Button
                                onPress={handleSubmitAndExit}
                                color="primary"
                                size="md"
                                radius="full"
                            >
                                Submit Exam
                            </Button>
                        </Link>
                    </div>
                </div>
            )}
            <div className="mx-5 mt-3  border-none px-8 h-full flex flex-col justify-between">
                <div className={`space-y-8 rounded-lg `}>
                    {currentQuestions.map((question) => (
                        <div key={question.questionId} className="space-y-4">
                            {question.questionType === "MCQ" &&
                                (() => {
                                    const mcqQuestion = question as MCQQuestion;
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
                                            #Question :
                                        </h2>
                                        <p>
                                            points: {(question as MCQQuestion).score}
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
                                            #Question :
                                        </h2>
                                        <p>
                                            points:
                                            {
                                                (question as WrittenQuestion)
                                                    .score
                                            }
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
                                            #Question :
                                        </h2>
                                        <p>
                                            points:
                                            {
                                                (question as ProblemQuestion)
                                                    .points
                                            }
                                        </p>
                                    </div>
                                    <CodeEditor
                                        examId={id?.toString()??""}
                                        question={question as ProblemQuestion}
                                        setAnswers={setAnswers}
                                        questionId={question.questionId}
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
