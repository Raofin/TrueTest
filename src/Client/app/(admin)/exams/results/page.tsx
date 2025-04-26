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
import PaginationButtons from '@/components/ui/pagination-button'

interface Exam {
    examId: string;
    title: string;
    description: string;
    totalPoints: number;
    durationMinutes: number;
    opensAt: string;
    closesAt: string;
    status: string;
    problemSolvingPoints: number;
    writtenPoints: number;
    mcqPoints: number;
}

interface Candidate {
    account: {
        accountId: string;
        username: string;
        email: string;
    };
    result: {
        totalScore: number;
        problemSolvingScore: number;
        writtenScore: number;
        mcqScore: number;
        startedAt: string;
        endedAt: string;
    };
}

 const ExamHeader = ({
    exams,
    selectedExamId,
    onSelectExam,
    candidates,
}: {
    exams: Exam[];
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
        <Card>
            <CardBody>
                <div className="flex w-full items-center justify-between mb-4">
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
                <div className="grid grid-cols-3 gap-4 mt-4">
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
                    <div className="flex items-center">
                        <p className="text-sm text-gray-500">
                            Invited Candidates :
                        </p>
                        <p className="font-medium">3068</p>
                    </div>
                </div>
            </CardBody>
        </Card>
    );
};
 const UserList = ({
    candidates,
    examId,
}: {
    candidates: Candidate[];
    examId: string;
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
    return (
        <div className="h-[70vh] flex flex-col justify-between">
            <div>
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
                <div className="overflow-x-auto">
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
                                        {user.result.totalScore}/100
                                    </td>
                                    <td className="py-3">
                                        <span className="px-2 py-1 rounded text-xs text-green-500">
                                            Reviewed
                                        </span>
                                    </td>
                                    <td className="py-3">
                                        {user.result.startedAt &&
                                        user.result.endedAt
                                            ? calculateDuration(
                                                  user.result.startedAt,
                                                  user.result.endedAt
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
            <div className="flex items-center justify-between mt-6">
                <div className="text-sm text-gray-400">
                    Page {currentPage} of {Math.ceil(candidates.length / 10)}
                </div>
                <div className="flex gap-2 items-center">
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
    const [exams, setExams] = useState<Exam[]>([]);
    const [selectedExamId, setSelectedExamId] = useState<string>("");
    const [candidates, setCandidates] = useState<Candidate[]>([]);

    useEffect(() => {
        const fetchExams = async () => {
            const response = await api.get("/Exam");
            if (response.status === 200) {
                setExams(response.data);
                if (response.data.length > 0) {
                    setSelectedExamId(response.data[0].examId);
                }
            }
        };
        fetchExams();
    }, []);

    useEffect(() => {
        const fetchCandidates = async () => {
            if (!selectedExamId) return;
            const response = await api.get(
                `/Review/Candidates/${selectedExamId}`
            );
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
                        onSelectExam={setSelectedExamId}
                        candidates={candidates}
                    />
                    <Card className="mt-6">
                        <CardBody>
                            <UserList
                                candidates={candidates}
                                examId={selectedExamId}
                            />
                        </CardBody>
                    </Card>
                </div>
            </div>
        </div>
    );
};

export default Component;

function calculateDuration(start: string, end: string): string {
    const startTime = new Date(start).getTime();
    const endTime = new Date(end).getTime();
    const durationMs = endTime - startTime;

    const hours = Math.floor(durationMs / (1000 * 60 * 60));
    const minutes = Math.floor((durationMs % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((durationMs % (1000 * 60)) / 1000);

    return `${hours}h ${minutes}m ${seconds}s`;
}
