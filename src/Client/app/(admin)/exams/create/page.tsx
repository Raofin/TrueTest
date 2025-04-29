"use client";

import React, { useEffect, useState } from "react";
import { Button, Card, Input, Textarea, TimeInput } from "@heroui/react";
import { CalendarDate, Time } from "@internationalized/date";
import { DatePicker } from "@heroui/date-picker";
import ProblemSolve from "@/components/ques/ProblemSolveQues";
import WrittenQues from "@/components/ques/WrittenQues";
import McqQues from "@/components/ques/McqQues";
import "@/app/globals.css";
import { v4 as uuidv4 } from "uuid";
import api from "@/lib/api";
import toast from "react-hot-toast";
import { useRouter, useSearchParams } from "next/navigation";
import { AxiosError } from "axios";
import { convertUtcToLocalTime, parseTime } from "@/components/DateTimeFormat";
import { AiButton } from '@/components/ui/AiButton'

interface FormData {
    title: string;
    description: string;
    durationMinutes: number;
    totalPoints: number;
    opensAt: string;
    closesAt: string;
}
export default function ExamFormPage() {
    const [date, setDate] = useState<CalendarDate | null>(null);
    const [problemQuesPoint, setProblemQuesPoint] = useState<number>(0);
    const [writtenQuesPoint, setWrittenQuesPoint] = useState<number>(0);
    const [mcqQuesPoint, setMcqQuesPoint] = useState<number>(0);
    const [totalPoints, setTotalPoints] = useState<number>(0);
    const [activeComponents, setActiveComponents] = useState<
        { id: string; type: string }[]
    >([]);
    const searchParams = useSearchParams();
    const route = useRouter();
    const [examId, setExamId] = useState(searchParams.get("id") || "");
    const isEdit = searchParams.get("isEdit") === "true";
    const [formData, setFormData] = useState<FormData>({
        title: "",
        description: "",
        durationMinutes: 0,
        totalPoints: 0,
        opensAt: "",
        closesAt: "",
    });
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const [saveStatus, setSaveStatus] = useState({
        exam: false,
        problemSolve: false,
        writtenQues: false,
        mcq: false,
    });
    const [questionData, setQuestionData] = useState({
        problemSolve: [],
        writtenQues: [],
        mcq: [],
    });
    const calculatedTotal = problemQuesPoint + writtenQuesPoint + mcqQuesPoint;
    const handleComponentSaved = (type: keyof typeof saveStatus) => {
        setSaveStatus((prev) => ({ ...prev, [type]: true }));
    };
    const handleAiResponse=()=>{
        const FetchData=async()=>{
        try{
          const response=await api.post('/Ai/Generate/ExamDescription',{
            title:formData.title,
            userPrompt:formData.description
          })
          if(response.status===200){
            setFormData({ ...formData, description: response.data.description });
          }
        }catch{}
        }
        FetchData();
    }
    const handleAddComponent = (componentType: string) => {
        setActiveComponents([
            ...activeComponents,
            { id: uuidv4(), type: componentType },
        ]);
    };
    const handleSaveExam = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!date) {
            toast.error("Please select a date");
            return;
        }
        try {
            const formatDateTimeToUTC = (
                timeString: string | undefined,
                selectedDate: CalendarDate | null
            ): string => {
                if (!timeString || !selectedDate) return "";
                const [hoursStr, minutesStr] = timeString.split(":");
                const hours = parseInt(hoursStr, 10);
                const minutes = parseInt(minutesStr, 10);
                const localDate = new Date(
                    selectedDate.year,
                    selectedDate.month - 1,
                    selectedDate.day,
                    hours,
                    minutes
                );
                return localDate.toISOString();
            };
            const examData = {
                title: formData.title,
                description: formData.description,
                durationMinutes: formData.durationMinutes,
                totalPoints: formData.totalPoints,
                opensAt: formatDateTimeToUTC(formData.opensAt, date),
                closesAt: formatDateTimeToUTC(formData.closesAt, date),
                date: new Date(
                    date.year,
                    date.month - 1,
                    date.day
                ).toISOString(),
            };
            setTotalPoints(examData.totalPoints);
            if (examData.opensAt >= examData.closesAt) {
                toast.error("please input correct start and end time");
            }
            let resp;
            if (examId) {
                const payload = {
                    examId: examId,
                    title: formData.title,
                    description: formData.description,
                    durationMinutes: formData.durationMinutes,
                    totalPoints: formData.totalPoints,
                    opensAt: formatDateTimeToUTC(formData.opensAt, date),
                    closesAt: formatDateTimeToUTC(formData.closesAt, date),
                };
                resp = await api.patch(`/Exam/Update`, payload);
                if (resp.status === 200) {
                    toast.success(`Exam updated successfully.`);
                    route.push("/view-exams");
                }
            } else {
                resp = await api.post("/Exam/Create", examData);
                if (resp.status === 200) {
                    toast.success(`Exam created successfully.`);
                    setExamId(resp.data.examId);
                }
            }
        } catch (err) {
            const error = err as AxiosError;
            toast.error(error?.message);
        }
    };
    useEffect(() => {
        const fetchExamDetails = async () => {
            try {
                if (!isEdit) {
                    return;
                }
                if (!examId) {
                    route.push("/view-exams");
                    return;
                }
                const [
                    examResponse,
                    problemQuesResponse,
                    writtenQuesResponse,
                    mcqQuesResponse,
                ] = await Promise.all([
                    api.get(`/Exam/${examId}`),
                    api.get(`/Questions/Problem/ByExam/${examId}`),
                    api.get(`/Questions/Written/ByExam/${examId}`),
                    api.get(`/Questions/Mcq/ByExam/${examId}`),
                ]);
                if (examResponse.status === 200) {
                    const exam = examResponse.data.exam;
                    setFormData({
                        title: exam.title,
                        description: exam.description,
                        durationMinutes: exam.durationMinutes,
                        totalPoints: exam.totalPoints,
                        opensAt: convertUtcToLocalTime(exam.opensAt),
                        closesAt: convertUtcToLocalTime(exam.closesAt),
                    });
                    const opensAtDate = new Date(exam.opensAt);
                    setDate(
                        new CalendarDate(
                            opensAtDate.getFullYear(),
                            opensAtDate.getMonth() + 1,
                            opensAtDate.getDate()
                        )
                    );
                }
                setQuestionData({
                    problemSolve: problemQuesResponse.data ?? [],
                    writtenQues: writtenQuesResponse.data ?? [],
                    mcq: mcqQuesResponse.data ?? [],
                });
                const components: { id: string; type: string }[] = [];

                if (problemQuesResponse.data?.length) {
                    components.push({ id: uuidv4(), type: "problemSolve" });
                }
                if (writtenQuesResponse.data?.length) {
                    components.push({ id: uuidv4(), type: "writtenQues" });
                }
                if (mcqQuesResponse.data?.length) {
                    components.push({ id: uuidv4(), type: "mcq" });
                }
                setActiveComponents(components);
            } catch (error) {
                console.error("Error fetching exam data:", error);
                toast.error("Failed to load exam data");
                route.push("/view-exams");
            }
        };

        fetchExamDetails();
    }, [examId, isEdit, route, totalPoints]);
    useEffect(() => {
        const debounceTimer = setTimeout(() => {
            const calculatedTotal =
                problemQuesPoint + writtenQuesPoint + mcqQuesPoint;
            if (calculatedTotal !== formData.totalPoints) {
                const message = `Points updated: 
            Problem Solving: ${problemQuesPoint}
            Written: ${writtenQuesPoint}
            MCQ: ${mcqQuesPoint}
            Total Calculated: ${calculatedTotal}
            Exam Total Points: ${formData.totalPoints}`;
                toast(message, {
                    duration: 4000,
                    position: "bottom-right",
                    style: {
                        whiteSpace: "pre-line",
                    },
                });
            }
        }, 1000);
        return () => clearTimeout(debounceTimer);
    }, [formData.totalPoints, mcqQuesPoint, problemQuesPoint, writtenQuesPoint]);
    const handlePublishExam = async () => {
        if (examId) {
            try {
                const response = await api.post(
                    `/Exam/Publish?examId=${examId}`
                );
                if (response.status === 200) {
                    toast.success("Exam published successfully.");
                    route.push("/view-exams");
                    resetForm();
                }
            } catch {
                toast.error(
                    `Failed to publish exam.Please make sure total points (${formData.totalPoints}) match sum of all questions (${calculatedTotal})`
                );
            }
        } else {
            toast.error("Please save the exam first.");
        }
    };
    const handleProblemPointsChange = (points: number) => {
        setProblemQuesPoint(points);
    };
    const handleWrittenPointsChange = (points: number) => {
        setWrittenQuesPoint(points);
    };
    const handleMcqPointsChange = (points: number) => {
        setMcqQuesPoint(points);
    };
    const handleTotalPointsChange = (
        e: React.ChangeEvent<HTMLInputElement>
    ) => {
        const value = parseInt(e.target.value);
        if (!isNaN(value)) {
            setFormData({
                ...formData,
                totalPoints: value,
            });
        }
    };
    const handleDeleteExam = async () => {
        if (examId) {
            const response = await api.delete(`/Exam/Delete/${examId}`);
            if (response.status === 200) {
                toast.success("Exam deleted successfully.");
                route.push("/view-exams");
            } else {
                toast.error("Failed to delete exam.");
            }
        }
    };
    const resetForm = () => {
        setFormData({
            title: "",
            description: "",
            durationMinutes: 0,
            totalPoints: 0,
            opensAt: "",
            closesAt: "",
        });
        setDate(null);
        setActiveComponents([]);
        setQuestionData({
            problemSolve: [],
            writtenQues: [],
            mcq: [],
        });
    };
    const handleOpenCloseTime = (time: Time | null): string => {
        if (!time) return "";
        return `${String(time.hour).padStart(2, "0")}:${String(
            time.minute
        ).padStart(2, "0")}`;
    };
    return (
        <>
            <div className="mx-44 flex flex-col gap-8">
                <h1 className="w-full text-center text-3xl font-bold my-3">
                    {isEdit ? "Edit Exam" : "Create Exam"}
                </h1>
                <Card className="flex shadow-none flex-col justify-between p-8 items-center">
                    <form
                        className="flex gap-4 flex-wrap flex-col w-full"
                        onSubmit={handleSaveExam}>
                        <Input
                            className="rounded-2xl"
                            isRequired
                            label="Title"
                            name="title"
                            type="text"
                            value={formData.title}
                            onChange={(e) =>
                                setFormData({
                                    ...formData,
                                    title: e.target.value,
                                })}/>
                        <div className='relative'>
                       <div>
                       <Textarea
                            className="rounded-2xl"
                            isRequired
                            label="Description"
                            name="description"
                            value={formData.description}
                            onChange={(e) =>
                                setFormData({
                                    ...formData,
                                    description: e.target.value,
                                })
                            }
                        />
                       </div>
                        <div className='absolute top-2 right-2'><AiButton onPress={handleAiResponse}/></div>
                        </div>
                        <div className="flex gap-5">
                            <DatePicker
                                className="flex-1 rounded-2xl"
                                isRequired
                                label="Exam Date"
                                value={date}
                                onChange={setDate}
                            />
                            <Input
                                className="flex-1 rounded-2xl"
                                isRequired
                                label="Duration (minutes)"
                                type="number"
                                min="1"
                                value={formData.durationMinutes.toString()}
                                onChange={(e) => {
                                    const value = parseInt(e.target.value);
                                    if (!isNaN(value)) {
                                        setFormData({
                                            ...formData,
                                            durationMinutes: value,
                                        });
                                    }
                                }}
                            />
                        </div>
                        <div className="flex gap-5">
                            <TimeInput
                                label="Start Time"
                                className="rounded-2xl"
                                value={parseTime(formData.opensAt)}
                                isRequired
                                onChange={(time) =>
                                    setFormData({
                                        ...formData,
                                        opensAt: handleOpenCloseTime(time),
                                    })
                                }
                                hourCycle={12}
                            />
                            <TimeInput
                                label="End Time"
                                className="rounded-2xl"
                                value={parseTime(formData.closesAt)}
                                isRequired
                                onChange={(time) =>
                                    setFormData({
                                        ...formData,
                                        closesAt: handleOpenCloseTime(time),
                                    })
                                }
                                hourCycle={12}
                            />
                        </div>
                        <Input
                            className="rounded-2xl"
                            isRequired
                            label="Total Points"
                            type="number"
                            min="1"
                            value={formData.totalPoints.toString()}
                            onChange={handleTotalPointsChange}
                        />
                        <div className="flex justify-end mt-2 gap-3">
                            <Button color="success" onPress={handlePublishExam}>
                                Publish
                            </Button>
                            <Button color="danger" onPress={handleDeleteExam}>
                                Delete
                            </Button>
                            <Button color="primary" type="submit">
                                {isEdit ? "Update" : "Save"}
                            </Button>
                        </div>
                    </form>
                </Card>
                {activeComponents.map((component) => (
                    <div key={component.id} className="w-full">
                        {component.type === "problemSolve" && (
                            <ProblemSolve
                                examId={examId}
                                existingQuestions={questionData.problemSolve}
                                onSaved={() =>
                                    handleComponentSaved("problemSolve")
                                }
                                problemPoints={handleProblemPointsChange}
                            />
                        )}
                        {component.type === "writtenQues" && (
                            <WrittenQues
                                examId={examId}
                                existingQuestions={questionData.writtenQues}
                                onSaved={() =>
                                    handleComponentSaved("writtenQues")
                                }
                                writtenPoints={handleWrittenPointsChange}
                            />
                        )}
                        {component.type === "mcq" && (
                            <McqQues
                                examId={examId}
                                existingQuestions={questionData.mcq}
                                onSaved={() => handleComponentSaved("mcq")}
                                mcqPoints={handleMcqPointsChange}
                            />
                        )}
                    </div>
                ))}
                {examId && (
                    <div className="flex gap-3 justify-center my-4">
                        {!activeComponents.some(
                            (c) => c.type === "problemSolve"
                        ) && (
                            <Button
                                onPress={() =>
                                    handleAddComponent("problemSolve")
                                }
                            >
                                Add Problem Solving Question
                            </Button>
                        )}
                        {!activeComponents.some(
                            (c) => c.type === "writtenQues"
                        ) && (
                            <Button
                                onPress={() =>
                                    handleAddComponent("writtenQues")
                                }
                            >
                                Add Written Question
                            </Button>
                        )}
                        {!activeComponents.some((c) => c.type === "mcq") && (
                            <Button onPress={() => handleAddComponent("mcq")}>
                                Add MCQ Question
                            </Button>
                        )}
                    </div>
                )}
            </div>
        </>
    );
}
