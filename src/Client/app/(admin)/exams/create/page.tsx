"use client";

import React, { useEffect, useState } from "react";
import { Button, Card, Input, Textarea, TimeInput } from "@heroui/react";
import { CalendarDate, Time } from "@internationalized/date";
import { DatePicker } from "@heroui/date-picker";
import ProblemSolve from "@/components/ques/problem-solving-ques";
import WrittenQues from "@/components/ques/written-ques";
import McqQues from "@/components/ques/mcq-ques";
import "@/app/globals.css";
import { v4 as uuidv4 } from "uuid";
import api from "@/lib/api";
import toast from "react-hot-toast";
import { useRouter, useSearchParams } from "next/navigation";
import { AxiosError } from "axios";
import { convertUtcToLocalTime } from "@/components/format-date-time";

interface FormData {
    title: string;
    description: string;
    durationMinutes: number;
    totalPoints: number;
    opensAt: string;
    closesAt: string;
}

function parseTime(time: string): Time | null {
    if (!time) return null;
    const [hour, minute] = time.split(":").map(Number);
    return new Time(hour, minute);
}

export default function CreateExamPage() {
    const [date, setDate] = useState<CalendarDate | null>(null);
    const [activeComponents, setActiveComponents] = useState<
        { id: string; type: string }[]
    >([]);
    const searchParams = useSearchParams();
    const route = useRouter();
    const [examId, setExamId] = useState(searchParams.get("id") ?? "");
    const isEdit = searchParams.get("isEdit") === "true";
    const [formData, setFormData] = useState<FormData>({
        title: "",
        description: "",
        durationMinutes: 0,
        totalPoints: 0,
        opensAt: "",
        closesAt: "",
    });
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

    const handleComponentSaved = (type: keyof typeof saveStatus) => {
        setSaveStatus((prev) => ({ ...prev, [type]: true }));
        console.log(saveStatus,type)
    };

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
            const formatDateTimeToUTC = (timeString: string | undefined, selectedDate: CalendarDate | null): string => {
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
                ...formData,
                opensAt: formatDateTimeToUTC(formData.opensAt, date),
                closesAt: formatDateTimeToUTC(formData.closesAt, date),
                date: new Date(date.year, date.month - 1, date.day).toISOString(),
            };
    
            let response;
            if (isEdit && examId) {
                response = await api.put(`/Exam/Update/${examId}`, examData);
            } else {
                response = await api.post("/Exam/Create", examData);
            }
    
            if (response.status === 200) {
                toast.success(`Exam ${isEdit ? 'updated' : 'created'} successfully.`);
                if (!isEdit) {
                    setExamId(response.data.examId);
                }
            }
        } catch (err) {
            const error = err as AxiosError;
            toast.error(error?.message);
        }
    };
    useEffect(() => {
        const FetchExamDetails = async () => {
            try {
                if (!examId || !isEdit) return;
                
                const examResponse = await api.get(`/Exam/${examId}`);
                const problemQuesResponse = await api.get(
                    `/Questions/Problem/ByExam/${examId}`
                );
                const writtenQuesResponse = await api.get(
                    `/Questions/Written/ByExam/${examId}`
                );
                const mcqQuesResponse = await api.get(
                    `/Questions/Mcq/ByExam/${examId}`
                );
    
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
                const components = [];
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
    
                setQuestionData({
                    problemSolve: problemQuesResponse.data || [],
                    writtenQues: writtenQuesResponse.data || [],
                    mcq: mcqQuesResponse.data || [],
                });
            } catch (error) {
                console.error("Error fetching exam or question data:", error);
                toast.error("Failed to load exam data");
            }
        };
        
        FetchExamDetails();
    }, [examId, isEdit]);
    const handlePublishExam = async () => {
        if (examId) {
            try {
                const response = await api.post(
                    `/Exam/Publish?examId=${examId}`
                );

                if (response.status === 200) {
                    toast.success("Exam published successfully.");
                }
            } catch {
                toast.error("Failed to publish exam");
            }
        }else 
        toast.error("Please save the exam first.")
    };
    const handleDeleteExam = async () => {
        if (examId) {
            try {
                const response = await api.delete(`/Exam/Delete/${examId}`);

                if (response.status === 200) {
                    toast.success("Exam deleted successfully.");
                    setFormData({
                        title: "",
                        description: "",
                        durationMinutes: 0,
                        totalPoints: 0,
                        opensAt: "",
                        closesAt: "",
                    });
                    setExamId("");
                    setDate(null);
                    route.push("/exams/create");
                }
            } catch {
                toast.error("Failed to delete exam");
            }
        }
    };
    const handleOpenCloseTime = (time: Time | null): string => {
        if (!time) return "";
        return `${String(time.hour).padStart(2, "0")}:${String(
            time.minute
        ).padStart(2, "0")}`;
    };
    return (
        <div className="mx-44 flex flex-col gap-8">
            <h1 className="w-full text-center text-3xl font-bold my-3">{`Create Exam`}</h1>
            <Card
                className={`flex shadow-none flex-col justify-between p-8 items-center `}
            >
                <form
                    id="#"
                    className="flex gap-4 flex-wrap flex-col w-full "
                    onSubmit={handleSaveExam}
                >
                    <Input
                        className="rounded-2xl"
                        isRequired
                        label="Title"
                        name="title"
                        type="text"
                        value={formData.title}
                        onChange={(e) =>
                            setFormData({ ...formData, title: e.target.value })
                        }
                    />
                    <Textarea
                        className="rounded-2xl"
                        isRequired
                        label="Description"
                        name="description"
                        type="text"
                        value={formData.description}
                        onChange={(e) =>
                            setFormData({
                                ...formData,
                                description: e.target.value,
                            })
                        }
                    />
                    <div className="flex gap-5">
                        <DatePicker
                            className="flex-1 rounded-2xl"
                            isRequired
                            label="Exam Date"
                            name="date"
                            value={date}
                            onChange={setDate}
                        />
                        <Input
                            className="flex-1 rounded-2xl"
                            isRequired
                            label="Duration(minute)"
                            name="duration"
                            type="number"
                            min="0"
                            value={formData.durationMinutes?.toString() ?? "0"}
                            onChange={(e) => {
                                const value = Number(e.target.value);
                                if (!isNaN(value) && value >= 1) {
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
                            name="opensAt"
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
                            className=" rounded-2xl"
                            name="closesAt"
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
                    <div>
                        <Input
                            className="flex-1  rounded-2xl"
                            isRequired
                            label="Total Points"
                            name="totalpoints"
                            type="number"
                            min="0"
                            value={formData.totalPoints?.toString() ?? "0"}
                            onChange={(e) => {
                                const value = Number(e.target.value);
                                if (!isNaN(value) && value >= 1) {
                                    setFormData({
                                        ...formData,
                                        totalPoints: value,
                                    });
                                }
                            }}

                        />
                    </div>
                    <div className="flex justify-end mt-2">
                        <Button
                            color="success"
                            onPress={handlePublishExam}>
                            Publish
                        </Button>

                        <Button
                            color="danger"
                            className="mx-3"
                            onPress={handleDeleteExam}
                        >
                            Delete
                        </Button>
                        <Button color="primary" type="submit">
                            Save
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
                            onSaved={() => handleComponentSaved("problemSolve")}
                        />
                    )}
                    {component.type === "writtenQues" && (
                        <WrittenQues
                            examId={examId}
                            existingQuestions={questionData.writtenQues}
                            onSaved={() => handleComponentSaved("writtenQues")}
                        />
                    )}
                    {component.type === "mcq" && (
                        <McqQues
                            examId={examId}
                            existingQuestions={questionData.mcq}
                            onSaved={() => handleComponentSaved("mcq")}
                        />
                    )}
                </div>
            ))}

            {examId && (
                <div className="flex gap-3 justify-center my-4">
                    {!activeComponents.some(
                        (comp) => comp.type === "problemSolve"
                    ) && (
                        <Button
                            onPress={() => handleAddComponent("problemSolve")}
                        >
                            Add Problem Solving Question
                        </Button>
                    )}
                    {!activeComponents.some(
                        (comp) => comp.type === "writtenQues"
                    ) && (
                        <Button
                            onPress={() => handleAddComponent("writtenQues")}
                        >
                            Add Written Question
                        </Button>
                    )}
                    {!activeComponents.some((comp) => comp.type === "mcq") && (
                        <Button onPress={() => handleAddComponent("mcq")}>
                            Add MCQ Question
                        </Button>
                    )}
                </div>
            )}
        </div>
    );
}