"use client";

import React, { useEffect, useState } from "react";
import { Button, Card, CardBody, CardHeader, Modal, ModalContent, ModalBody, useDisclosure, } from "@heroui/react";
import axios from "axios";
import { Controller, useForm } from "react-hook-form";
import PaginationButtons from "@/app/components/ui/pagination-button";

interface Exam {
    examId: string;
    title: string;
    description: string;
    durationMinutes: number;
    opensAt: string;
    createdAt: string;
    closesAt: string;
    status: "Upcoming" | "Running" | "Ended";
    invitedCandidates: number | "N/A";
    acceptedCandidates: number | "N/A";
    problemSolving: number;
    written: number;
    mcq: number;
    score: number;
}

const ITEMS_PER_PAGE = 2;
const Exams: Exam[] = [
    {
        title: "Star Coder 2026",
        description: "Running",
        durationMinutes: 60,
        opensAt: "2026-11-21T21:00:00.000Z",
        closesAt: "2026-11-21T22:20:00.000Z",
        status: "Running",
        problemSolving: 10,
        written: 10,
        mcq: 30,
        score: 100,
        examId: "",
        createdAt: "",
        invitedCandidates: 0,
        acceptedCandidates: 0
    },
    {
        title: "Learnathon 4.0",
        description: "Upcoming",
        durationMinutes: 90,
        opensAt: "2026-12-10T21:00:00.000Z",
        closesAt: "2026-12-10T23:00:00.000Z",
        status: "Upcoming",
        problemSolving: 10,
        written: 10,
        mcq: 30,
        score: 100,
        examId: "",
        createdAt: "",
        invitedCandidates: 0,
        acceptedCandidates: 0
    },
    {
        title: "Star Coder 2005",
        description: "Ended",
        durationMinutes: 60,
        opensAt: "2025-11-21T21:00:00.000Z",
        closesAt: "2025-11-21T22:00:00.000Z",
        status: "Ended",
        problemSolving: 10,
        written: 10,
        mcq: 30,
        score: 100,
        examId: "",
        createdAt: "",
        invitedCandidates: 0,
        acceptedCandidates: 0
    },
];
export default function ExamList() {
    const { isOpen, onOpen, onOpenChange } = useDisclosure();
    const { control, handleSubmit, reset } = useForm<Exam>();
    const Mode = localStorage.getItem("theme")
    const [currentPage, setCurrentPage] = useState(1);
    // const [exams, setExams] = useState<Exam[]>([]);
    const [selectedExam, setSelectedExam] = useState<Exam | null>(null);
    const totalPages = Math.max(1, Math.ceil(Exams.length / ITEMS_PER_PAGE));

    useEffect(() => {
        setCurrentPage((prev) => Math.min(prev, totalPages));
    }, [Exams, totalPages]);
    const paginatedExams = Exams.slice(
        (currentPage - 1) * ITEMS_PER_PAGE,
        currentPage * ITEMS_PER_PAGE
    );

    // const convertUTCToLocal = (utcString: string) => {
    //     console.log(utcString)
    //     const date = new Date(utcString);
    //     return date.toLocaleString();
    // };

    // const convertLocalToUTC = (localTime: string) => {
    //     if (!localTime) return "";
    //     const date = new Date(localTime);
    //     console.log("Local Time:", localTime, "-> UTC Time:", date.toISOString());
    //     return date.toISOString();
    // };
    const handleEdit = (exam: Exam) => {
        setSelectedExam(exam);
        reset(exam);
        onOpen();
    };
    const handleDelete = async (examId: string) => {

        // try {
        //   const response = await axios.delete(
        //     `${process.env.NEXT_PUBLIC_URL}/Exam/${examId}`
        //   );

        //   if (response.status === 200) {
        //     setExams((prevExams) =>
        //       prevExams.filter((exam) => exam.examId !== examId)
        //     );
        //     console.log("Exam deleted successfully!");
        //   } else {
        //     console.error("Failed to delete exam:", response.data);
        //     alert(
        //       `Failed to delete exam: ${response.data.message || "Unknown error"
        //       }`
        //     );
        //   }
        // } catch (error:any) {
        //   if (error.response) {
        //     console.error("Error deleting exam:", error.response.data);
        //   } else if (error.request) {
        //     console.error("Error deleting exam: No response received");
        //     alert("Failed to delete exam: No response from server");
        //   } else {
        //     console.error("Error deleting exam:", error.message);
        //     alert(`Failed to delete exam: ${error.message}`);
        //   }
        // }

    };
    // const onSubmit = async (data: Exam) => {
    //     if (!selectedExam) return;

    //     const updatedExam = {
    //         ...data,
    //         opensAt: convertLocalToUTC(data.opensAt),
    //         closesAt: convertLocalToUTC(data.closesAt),
    //     };

    //     try {
    //         const response = await axios.put(
    //             `${process.env.NEXT_PUBLIC_URL}/Exam/${selectedExam.examId}`,
    //             updatedExam
    //         );

    //         if (response.status === 200) {
    //             setExams((prevExams) =>
    //                 prevExams.map((exam) =>
    //                     exam.examId === selectedExam.examId ? { ...exam, ...updatedExam } : exam
    //                 )
    //             );
    //             console.log("Exam updated successfully!");
    //             onOpenChange();
    //         } else {
    //             console.error("Failed to update exam:", response.data);
    //             alert(`Failed to update exam: ${response.data.message || "Unknown error"}`);
    //         }
    //     } catch (error:any) {
    //         if (error.response) {
    //             console.error("Error updating exam:", error.response.data);
    //             alert(`Failed to update exam: ${error.response.data.message || "Server error"}`);
    //         } else if (error.request) {
    //             console.error("Error updating exam: No response received");
    //             alert("Failed to update exam: No response from server");
    //         } else {
    //             console.error("Error updating exam:", error.message);
    //             alert(`Failed to update exam: ${error.message}`);
    //         }
    //     }
    // };

    useEffect(() => {
        if (currentPage > totalPages) {
            setCurrentPage(totalPages > 0 ? totalPages : 1);
        }
    }, [Exams, totalPages]);

    // useEffect(() => {
    //     const fetchExamList = async () => {
    //         try {
    //             const response = await axios.get(`${process.env.NEXT_PUBLIC_URL}/Exam`);

    //             if (response.status === 200) {
    //                 const examData = response.data.map((exam: any) => ({
    //                     ...exam,
    //                     opensAt: convertUTCToLocal(exam.opensAt),
    //                     closesAt: convertUTCToLocal(exam.closesAt),
    //                 }));

    //                 setExams(examData);
    //             } else {
    //                 console.error("Failed to fetch exams.");
    //             }
    //         } catch (error) {
    //             console.error("Error fetching exams:", error);
    //         }
    //     };

    //     fetchExamList();
    // }, []);

    return (
        <>

            <div className={`h-screen flex flex-col justify-around ${Mode === "dark" ? "bg-black" : "bg-white"}`}>
                {paginatedExams.map((exam, index) => (
                    <Card key={index} className="mx-8">
                        <CardHeader className="flex justify-between items-center ">
                            <h1 className="text-2xl font-bold flex gap-1 items-end">{exam.title}</h1>
                            <div className="flex gap-2">
                                {exam.status === "Ended" ? (
                                    <Button className="bg-purple-500 text-white">Review</Button>
                                ) : exam.status === "Upcoming" ? (
                                    <>
                                        <Button color="primary" onPress={() => handleEdit(exam)}>Edit</Button>
                                        <Button className="bg-blue-500 text-white">Publish</Button>
                                    </>
                                ) : (<>
                                    <Button color="primary" onPress={() => handleEdit(exam)}>Edit</Button>
                                    <Button color="primary" onPress={() => handleDelete(exam.examId)}>Delete</Button>
                                </>)}
                            </div>
                        </CardHeader>
                        <CardBody className="px-3">
                            <div className="flex">
                                <div className="flex flex-col flex-1">
                                    <p>Date: Friday, 21 Nov 2026</p>
                                    <p>Duration: 2 hr</p>
                                    <p>Starts at: 9:00 PM</p>
                                    <p>Closes at: 10:20 PM</p>
                                </div>
                                <div className="flex flex-col flex-1">
                                    <p>Problem Solving: 10</p>
                                    <p>Written: 10</p>
                                    <p>MCQ: 10</p>
                                    <p>Score: 100</p>
                                </div>
                                <div className="flex flex-col flex-1">
                                    <p>Invited Candidates: 120</p>
                                    <p>Accepted: 90</p>
                                </div>
                            </div>
                        </CardBody>
                    </Card>
                ))}
                <div className="flex justify-center items-center my-4">
                    <span className="mx-4">Page {currentPage} of {totalPages}</span>
                    <PaginationButtons
                        currentIndex={currentPage}
                        totalItems={totalPages}
                        onPrevious={() => setCurrentPage((prev) => Math.max(1, prev - 1))}
                        onNext={() => setCurrentPage((prev) => Math.min(totalPages, prev + 1))} />
                </div>
            </div>

            <Modal isOpen={isOpen} placement="top-center" onOpenChange={onOpenChange}>
                <ModalContent>
                    <ModalBody>
                        {selectedExam && (
                            <form
                                // onSubmit={handleSubmit(onSubmit)}
                                className="flex flex-col gap-6 p-10">
                                <Controller name="title" control={control} render={({ field }) => <div className="flex gap-2"><h3 className="font-semibold">Title :</h3> <input {...field} placeholder="Exam Title" /></div>} />
                                <Controller name="description" control={control} render={({ field }) => <div className="flex gap-2"><h3 className="font-semibold">Description :</h3> <input {...field} placeholder="Description" /></div>} />
                                <Controller name="durationMinutes" control={control} render={({ field }) => <div className="flex gap-2"><h3 className="font-semibold flex">Duration(min) :</h3> <input {...field} type="number" placeholder="Duration (minutes)" /></div>} />
                                <Controller name="opensAt" control={control} render={({ field }) => <div className="flex gap-2"><h3 className="font-semibold">Start Time :</h3> <input {...field} placeholder="Start Time" /></div>} />
                                <Controller name="closesAt" control={control} render={({ field }) => <div className="flex gap-2"><h3 className="font-semibold">Close Time :</h3><input {...field} placeholder="Close Time" /></div>} />
                                <Button color="primary" type="submit">Update Exam</Button>
                            </form>
                        )}
                    </ModalBody>
                </ModalContent>
            </Modal>
        </>
    );
}