"use client";

import api from "@/lib/api";
import {
    Button,
    Card,
    CardBody,
    Checkbox,
    Input,
    Select,
    SelectItem,
} from "@heroui/react";
import { Icon } from "@iconify/react";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import PaginationButtons from "@/components/ui/PaginationButton";
import { ExamData } from "@/components/types/exam";
import { Candidate } from "@/components/types/result";
import { calculateDuration } from "@/components/DateTimeFormat";

const ExamHeader = ({
    exams,
    selectedExamId,
    onSelectExam,
    candidates,
}: {
    exams: ExamData[];
    selectedExamId: string;
    onSelectExam: (examId: string) => void;
    candidates: Candidate[];
}) => {
    const selectedExam = exams.find((exam) => exam.examId === selectedExamId);
    const handlePrevExam = () => {
        const currentIndex = exams.findIndex(
            (c) => c.examId === selectedExamId
        );
        if (currentIndex > 0) {
            onSelectExam(exams[currentIndex - 1].examId);
        }
    };

    const handleNextExam = () => {
        const currentIndex = exams.findIndex(
            (c) => c.examId === selectedExamId
        );
        if (currentIndex < exams.length - 1) {
            onSelectExam(exams[currentIndex + 1].examId);
        }
    };
    return (
        <Card className='shadow-none'>
            <CardBody>
                <div className="flex w-full items-center justify-between mb-4 px-3">
                    <div className="flex items-center gap-2">
                        <h2 className="text-lg font-medium">Exam</h2>
                        <Select
                            aria-label="Select an exam"
                            className="w-[250px]"
                            value={selectedExamId || "select an exam"}
                            onChange={(e: { target: { value: string } }) =>
                                onSelectExam(e.target.value)
                            }
                        >
                            {exams.map((exam) => (
                                <SelectItem key={exam.examId}>
                                    {exam.title}
                                </SelectItem>
                            ))}
                        </Select>
                    </div>
                    <div className="flex gap-2">
                        <Button
                            variant="flat"
                            size="sm"
                            onPress={handlePrevExam}
                        >
                            Previous
                        </Button>
                        <Button
                            variant="flat"
                            size="sm"
                            onPress={handleNextExam}
                        >
                            Next
                        </Button>
                    </div>
                </div>
                <div className="w-full flex justify-between mt-4 px-3">
                    <div className="flex items-center">
                        <p className="text-sm text-gray-500">Date :</p>
                        <p className="font-medium">
                            {selectedExam?.opensAt
                                ? new Date(
                                      selectedExam.opensAt
                                  ).toLocaleString()
                                : "N/A"}
                        </p>
                    </div>
                    <div className="flex items-center">
                        <p className="text-sm text-gray-500">Attended :</p>
                        <p className="font-medium">{candidates.length}</p>
                    </div>
                </div>
            </CardBody>
        </Card>
    );
};
const UserList = ({
    candidates,
    examId,
    exams,
    selectedExamTitle,
}: {
    candidates: Candidate[];
    examId: string;
    exams:ExamData[]
    selectedExamTitle: string;
}) => {
    const [selectedUsers, setSelectedUsers] = useState<string[]>([]);
    const [currentPage, setCurrentPage] = useState(1);

    const handleCheckboxChange = (email: string) => {
        if (selectedUsers.includes(email)) {
            setSelectedUsers(selectedUsers.filter((u) => u !== email));
        } else {
            setSelectedUsers([...selectedUsers, email]);
        }
    };

    const handleSelectAll = (checked: boolean) => {
        if (checked) {
            setSelectedUsers(candidates.map((user) => user.account.email));
        } else {
            setSelectedUsers([]);
        }
    };
    const router = useRouter();
    const handleReviewPage = (selectedCandidate: string) => {
        router.push(
            `/exams/review?examId=${examId}&candidateId=${selectedCandidate}`
        );
    };
    const [examTitleDate,setExamTitleDate]=useState<ExamData>()
    useEffect(()=>{
        setExamTitleDate(exams.find((e:ExamData)=>e.examId===examId))
    },[examId,exams])
    return (
        <div className="h-[70vh] flex flex-col justify-between">
            <div>
                <h1 className="w-full flex justify-center text-xl font-semibold">
                    Exam: {selectedExamTitle || "No Exam Selected"}
                </h1>
                <div className="flex w-full items-center justify-between p-3 mb-4">
                    <h3 className="text-lg font-medium">User List</h3>
                    <Input
                        className="w-[300px]"
                        placeholder="Search"
                        endContent={
                            <Icon
                                icon="lucide:search"
                                className="text-gray-400"
                            />
                        }
                    />
                </div>
                <div className="overflow-x-auto p-3">
                    <table className="w-full">
                        <thead>
                            <tr className="text-left">
                                <th className="pb-2 pl-2">
                                    <Checkbox
                                        isSelected={
                                            selectedUsers.length ===
                                                candidates.length &&
                                            candidates.length > 0
                                        }
                                        onValueChange={handleSelectAll}
                                    />
                                </th>
                                <th className="pb-2">Email</th>
                                <th className="pb-2">Score</th>
                                <th className="pb-2">Review Status</th>
                                <th className="pb-2">Duration</th>
                                <th className="pb-2">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {candidates.map((user) => (
                                <tr key={user.account.accountId}>
                                    <td className="py-3 pl-2">
                                        <Checkbox
                                            isSelected={selectedUsers.includes(
                                                user.account.email
                                            )}
                                            onValueChange={() =>
                                                handleCheckboxChange(
                                                    user.account.email
                                                )
                                            }
                                        />
                                    </td>
                                    <td className="py-3">
                                        {user.account.email}
                                    </td>
                                    <td className="py-3">
                                        {user.result?.totalScore || 0}/{examTitleDate?.totalPoints}
                                    </td>
                                    <td className="py-3">
                                        <span className="px-2 py-1 rounded text-xs text-green-500">
                                            {user.result?.totalScore ? "Reviewed":"Pending"}
                                        </span>
                                    </td>
                                    <td className="py-3">
                                        {user.result?.startedAt &&
                                        user.result?.submittedAt
                                            ? calculateDuration(
                                                  user.result?.startedAt,
                                                  user.result?.submittedAt
                                              )
                                            : "N/A"}
                                    </td>
                                    <td className="py-3">
                                        <Button
                                            onPress={() =>
                                                handleReviewPage(
                                                    user.account.accountId
                                                )
                                            }
                                            size="sm"
                                            color="success"
                                            className="text-xs"
                                        >
                                            Review Results
                                        </Button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
            <div className="flex items-center justify-between mt-6 px-3">
                <div className="text-sm text-gray-400">
                    Page {currentPage} of {Math.ceil(candidates.length / 10)}
                </div>
                <div className="flex gap-2 items-center pb-2">
                    <PaginationButtons
                        currentIndex={currentPage}
                        totalItems={Math.ceil(candidates.length / 10)}
                        onPrevious={() =>
                            setCurrentPage((prev) => Math.max(prev - 1, 1))
                        }
                        onNext={() =>
                            setCurrentPage((prev) =>
                                Math.min(
                                    prev + 1,
                                    Math.ceil(candidates.length / 10)
                                )
                            )
                        }
                    />
                    <Button size="sm" color="primary">
                        Send Email
                    </Button>
                </div>
            </div>
        </div>
    );
};

const Component: React.FC = () => {
    const [exams, setExams] = useState<ExamData[]>([]);
    const [selectedExamId, setSelectedExamId] = useState<string>("");
    const [candidates, setCandidates] = useState<Candidate[]>([]);
    const [selectedExamTitle, setSelectedExamTitle] = useState<string>("");
    const handleExamChange = (examId: string) => {
        const selectedExam = exams.find((exam) => exam.examId === examId);
        if (selectedExam) {
            setSelectedExamId(examId);
            setSelectedExamTitle(selectedExam.title);
        }
    };

    useEffect(() => {
        const fetchExams = async () => {
            const response = await api.get("/Exam");
            if (response.status === 200) {
                setExams(response.data);
                if (response.data.length > 0) {
                    setSelectedExamId(response.data[0].examId);
                    setSelectedExamTitle(response.data[0].title);
                }
            }
        };
        fetchExams();
    }, []);
    useEffect(() => {
        const fetchCandidates = async () => {
            if (!selectedExamId) return;
            const response = await api.get(`/Review/Candidates/${selectedExamId}`);
            if (response.status === 200) {
                setCandidates(response.data.candidates || []);
            }
        };
        fetchCandidates();
    }, [selectedExamId]);

    return (
        <div className="h-full w-full bg-gray-100 dark:bg-gray-900">
            <h1 className="w-full text-center text-xl font-semibold mt-3">
                Results
            </h1>
            <div className="flex w-full p-6">
                <div className="w-full mx-44">
                    <ExamHeader
                        exams={exams}
                        selectedExamId={selectedExamId}
                        onSelectExam={handleExamChange}
                        candidates={candidates}
                    />
                    <Card className="mt-6 shadow-none">
                        <CardBody>
                            <UserList
                                candidates={candidates}
                                examId={selectedExamId}
                                selectedExamTitle={selectedExamTitle}
                                exams={exams}
                            />
                        </CardBody>
                    </Card>
                </div>
            </div>
        </div>
    );
};

export default Component;
