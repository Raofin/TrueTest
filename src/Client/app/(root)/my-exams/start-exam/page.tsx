"use client";

import { Card, CardBody, Checkbox, Button } from "@heroui/react";
import { useState } from "react";
import RootNavBar from "@/app/(root)/root-navbar";
import {
    convertUtcToLocalTime,
    FormattedDateWeekday,
    formatTimeHourMinutes,
} from "@/components/DateTimeFormat";
import { useRouter, useSearchParams } from "next/navigation";
import { ExamData } from '@/components/types/exam'

export default function StartExam() {
    const router = useRouter();
    const searchParams = useSearchParams();
    const examDataString = searchParams.get("examData");
    const currentExam: ExamData | undefined = examDataString
        ? JSON.parse(examDataString)
        : undefined;

    const [agreedToTerms, setAgreedToTerms] = useState({
        capture: false,
        screenMonitor: false,
        audio: false,
        clipboard: false,
    });

    if (!currentExam?.closesAt) return null;
    const handleStartExam = () => {
        if (currentExam?.examId) {
            router.push(`/my-exams/${currentExam.examId}`);
        }
    };
    const now = new Date();
    const closesAt = new Date(currentExam.closesAt);
    const nowUTC = new Date(
        now.getUTCFullYear(),
        now.getUTCMonth(),
        now.getUTCDate(),
        now.getUTCHours(),
        now.getUTCMinutes(),
        now.getUTCSeconds()
    );
    const timeLeftInMs = closesAt.getTime() - nowUTC.getTime();
    const isClosed = timeLeftInMs <= 0;
    const absTimeLeft = Math.abs(timeLeftInMs);
    const minutesLeft = isClosed
        ? 0
        : Math.floor((absTimeLeft / (1000 * 60)) % 60);
    const hoursLeft = isClosed ? 0 : Math.floor(absTimeLeft / (1000 * 60 * 60));

    return (
        <div>
            <RootNavBar />
            <div className="min-h-screen flex flex-col items-center justify-center">
                <Card className=" max-w-3xl mx-auto p-3 border-none shadow-none bg-white dark:bg-[#18181b]">
                    <CardBody>
                        <div className="space-y-3 ">
                            <div className="flex w-full justify-between">
                                <h1 className="text-2xl font-bold w-full">
                                    {currentExam?.title}
                                    <span
                                        className={`ml-2 text-sm text-red-500`}
                                    >
                                        {currentExam?.status}
                                    </span>
                                </h1>
                                <div className="flex items-center gap-1 before:content-[''] before:w-2 before:h-2 before:bg-red-500 before:rounded-full">
                                    <p className="min-w-[100px] text-sm">
                                        {isClosed
                                            ? ""
                                            : `${hoursLeft}h ${minutesLeft}m left`}
                                    </p>
                                </div>
                            </div>

                            <div className="grid grid-cols-2 gap-2 text-sm">
                                <div>
                                    <span className="text-[#71717a] dark:text-white">
                                        Date:
                                    </span>
                                    {currentExam?.closesAt && (
                                        <FormattedDateWeekday
                                            date={currentExam?.closesAt}
                                        />
                                    )}
                                </div>
                                <div className="text-right">
                                    <span className="text-[#71717a] dark:text-white">
                                        Problem Solving:
                                    </span>{" "}
                                    {currentExam?.problemSolvingPoints}
                                </div>
                                <div>
                                    <span className="text-[#71717a] dark:text-white">
                                        Starts at:
                                    </span>
                                    {currentExam?.opensAt &&
                                        convertUtcToLocalTime(
                                            currentExam.opensAt
                                        )}
                                </div>
                                <div className="text-right">
                                    <span className="text-[#71717a] dark:text-white">
                                        Written:
                                    </span>{" "}
                                    {currentExam?.writtenPoints}
                                </div>
                                <div>
                                    <span className="text-[#71717a] dark:text-white">
                                        Closes at:
                                    </span>{" "}
                                    {currentExam?.closesAt &&
                                        convertUtcToLocalTime(
                                            currentExam.closesAt
                                        )}
                                </div>
                                <div className="text-right">
                                    <span className="text-[#71717a] dark:text-white">
                                        MCQ:
                                    </span>
                                    {currentExam?.mcqPoints}
                                </div>
                                <div>
                                    <span className="text-[#71717a] dark:text-white">
                                        Duration:
                                    </span>
                                    {formatTimeHourMinutes(currentExam?.durationMinutes )}hr
                                </div>
                                <div className="text-right">
                                    <span className="text-[#71717a] dark:text-white">
                                        Score:
                                    </span>
                                    {currentExam?.totalPoints}
                                </div>
                            </div>
                            <hr className="my-2" />
                            <div className="space-y-2">
                                <h2 className="text-lg font-semibold">
                                    Instructions
                                </h2>
                                <ol className="list-decimal list-inside space-y-2 text-sm">
                                    <li>
                                        Please ensure your webcam and microphone
                                        are enabled throughout the exam.
                                    </li>
                                    <li>
                                        There should be no one else visible in
                                        the webcam area during the exam.
                                    </li>
                                    <li>
                                        A stable internet connection is
                                        required. If you are disconnected for
                                        more than 2 minutes, your answers will
                                        be automatically submitted.
                                    </li>
                                    <li>
                                        Your face must be clearly visible in the
                                        webcam at all times, and your eyes
                                        should remain on the screen. Please do
                                        not look away from the screen.
                                    </li>
                                    <li>
                                        The exam will automatically switch you
                                        to full-screen mode. Please do not try
                                        to change it.
                                    </li>
                                    <li>
                                        Do not attempt to copy or paste anything
                                        during the exam. It will be detected and
                                        flagged.
                                    </li>
                                </ol>
                            </div>
                            <div className="space-y-1">
                                <h3 className="font-semibold">
                                    Please read and agree to the following terms
                                    to proceed with the exam:
                                </h3>
                                <div className="space-y-2 text-sm">
                                    <Checkbox
                                        isSelected={agreedToTerms.capture}
                                        onValueChange={(value) =>
                                            setAgreedToTerms((prev) => ({
                                                ...prev,
                                                capture: value,
                                            }))
                                        }
                                    >
                                        I agree that images will be captured of
                                        me every 5 seconds.
                                    </Checkbox>
                                    <Checkbox
                                        isSelected={agreedToTerms.screenMonitor}
                                        onValueChange={(value) =>
                                            setAgreedToTerms((prev) => ({
                                                ...prev,
                                                screenMonitor: value,
                                            }))
                                        }
                                    >
                                        I agree that my screen will be monitored
                                        during the exam.
                                    </Checkbox>
                                    <Checkbox
                                        isSelected={agreedToTerms.audio}
                                        onValueChange={(value) =>
                                            setAgreedToTerms((prev) => ({
                                                ...prev,
                                                audio: value,
                                            }))
                                        }
                                    >
                                        I agree that my audio will be recorded
                                        throughout the exam.
                                    </Checkbox>
                                    <Checkbox
                                        isSelected={agreedToTerms.clipboard}
                                        onValueChange={(value) =>
                                            setAgreedToTerms((prev) => ({
                                                ...prev,
                                                clipboard: value,
                                            }))
                                        }
                                    >
                                        I agree that my clipboard activity will
                                        be tracked.
                                    </Checkbox>
                                </div>
                            </div>
                            <div className="w-full text-center">
                                <Button
                                    color="primary"
                                    isDisabled={
                                        !Object.values(agreedToTerms).every(
                                            Boolean
                                        )
                                    }
                                    onPress={handleStartExam}
                                >
                                    Start Exam
                                </Button>
                            </div>
                        </div>
                    </CardBody>
                </Card>
            </div>
        </div>
    );
}
