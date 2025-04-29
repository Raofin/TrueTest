"use client";

import React, { useState, useEffect } from "react";
import { Card, Button, Textarea, Select, SelectItem } from "@heroui/react";
import api from "@/lib/api";
import { useSearchParams } from "next/navigation";
import ReactMarkdown from "react-markdown";
import toast from "react-hot-toast";
import { CandidateData, CandidatesResponse, CandidateSubmission, ExamResponse, ProblemSubmission, WrittenSubmission } from '@/components/types/review'
import { ExamData } from '@/components/types/exam'

export default function Component() {
    const [problemPoints, setProblemPoints] = useState<number | undefined>();
    const [writtenPoints, setWrittenPoints] = useState<number | undefined>();
    const [mcqPoints, setMcqPoints] = useState<number | undefined>();
    const [mcqScore, setMcqScore] = useState(0);
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const [questionsData, setQuestionsData] = useState<Record<string, any>>({});
    const [totalPoints, setTotalPoints] = useState<number | undefined>();
    const [candidateList, setCandidateList] = useState<CandidateData[]>([]);
    const [editedSubmission, setEditedSubmission] = useState<CandidateSubmission | null>(null);
    const searchParams = useSearchParams();
    const examId = searchParams.get("examId");
    const candidateId = searchParams.get("candidateId");
    const [selectedCandidateId, setSelectedCandidateId] = useState<string>(candidateId||"");
    const [exam,setExam]=useState<ExamData>()
    useEffect(() => {
        const fetchExamData = async () => {
            if (!examId) return;
            try {
                const response = await api.get<ExamResponse>(`/Exam/${examId}`);
                if (response.status === 200) {
                    const { exam, questions } = response.data;
                    // eslint-disable-next-line @typescript-eslint/no-explicit-any
                    const newQuestionsData: Record<string, any> = {};
                    questions.problem.forEach((q: ProblemSubmission) => {
                        newQuestionsData[q.questionId] = {
                            ...q,
                            questionType: "problem",
                        };
                    });
                    questions.written.forEach((q: WrittenSubmission) => {
                        newQuestionsData[q.questionId] = {
                            ...q,
                            questionType: "written",
                        };
                    });
                    setExam(exam)
                    setQuestionsData(newQuestionsData);
                    setProblemPoints(exam.problemSolvingPoints);
                    setWrittenPoints(exam.writtenPoints);
                    setMcqPoints(exam.mcqPoints);
                    setTotalPoints(exam.totalPoints);
                }
            } catch (error) {
                console.error("Error fetching exam data:", error);
            }
        };
        fetchExamData();
    }, [examId]);
    useEffect(() => {
        const fetchCandidateData = async () => {
            try {
                const response = await api.get<CandidatesResponse>(`/Review/Candidates/${examId}`);
                if (response.status === 200) {
                    setCandidateList(response.data.candidates);
                    console.log(response.data)
                    if (response.data.candidates.length > 0) {
                        const candidateExists = response.data.candidates.some(
                            (c) => c.account.accountId === selectedCandidateId );
                        if (!candidateExists) {
                            setSelectedCandidateId(response.data.candidates[0].account.accountId);
                        }
                    } }
            } catch (error) {
                console.error("Error fetching candidate data:", error);
            }};
        if (examId) fetchCandidateData();
    }, [examId, selectedCandidateId]);

    useEffect(() => {
        const fetchSubmissionData = async () => {
            try {
                if (!selectedCandidateId || !examId) return;
                const response = await api.get(`/Review/Candidates/${examId}/${selectedCandidateId}`);
                if (response.status === 200) {
                    const submissionData = response.data;
                    const processedData = {
                        problem: submissionData.submission.problem.map((p: ProblemSubmission) => ({
                            ...p,
                            isFlagged: p.isFlagged || false,
                            flagReason: p.flagReason || null,
                        })),
                        written: submissionData.submission.written.map((w: WrittenSubmission) => ({
                            ...w,
                            isFlagged: w.isFlagged || false,
                            flagReason: w.flagReason || null,
                        })),
                    };
                    setEditedSubmission(processedData);
                }
            } catch (error) {
                console.error("Error fetching submission data:", error);
            }
        };
        fetchSubmissionData();
    }, [examId, selectedCandidateId]);

    const handleCandidateChange = (value: string) => {
        setSelectedCandidateId(value);
        setMcqScore(0)
    };
    const handlePrevCandidate = () => {
        const currentIndex = candidateList.findIndex(
            (c) => c.account.accountId === selectedCandidateId
        );
        if (currentIndex > 0) {
            setSelectedCandidateId(
                candidateList[currentIndex - 1].account.accountId
            );
        }
        setMcqScore(0)
    };
    const handleNextCandidate = () => {
        const currentIndex = candidateList.findIndex(
            (c) => c.account.accountId === selectedCandidateId
        );
        if (currentIndex < candidateList.length - 1) {
            setSelectedCandidateId(
                candidateList[currentIndex + 1].account.accountId
            );
        }
        setMcqScore(0)
    };
const updateProblemSubmission = (
    questionId: string,
    updates: Partial<ProblemSubmission>
  ) => {
    setEditedSubmission((prev) => {
      if (!prev) return null;
      
      const updatedProblem = prev.problem.map(p => 
        p.questionId === questionId ? {...p, ...updates} : p
      );
      const newProblemScore = updatedProblem.reduce((sum, p) => sum + p.score, 0);
      setCandidateList(prevCandidates => prevCandidates.map(candidate => {
        if (candidate.account.accountId === selectedCandidateId) {
          return {
            ...candidate,
            result: {
              ...candidate.result,
              problemSolvingScore: newProblemScore,
              totalScore: newProblemScore + 
                         candidate.result.writtenScore + mcqScore 
            }
          };
        }
        return candidate;
      }));
  
      return {...prev, problem: updatedProblem};
    });
  };
  
  const updateWrittenSubmission = (
    questionId: string,
    updates: Partial<WrittenSubmission>
  ) => {
    setEditedSubmission((prev) => {
      if (!prev) return null;
  
      const updatedWritten = prev.written.map(w => 
        w.questionId === questionId ? {...w, ...updates} : w
      );
      const newWrittenScore = updatedWritten.reduce((sum, w) => sum + w.score, 0)
      setCandidateList(prevCandidates => prevCandidates.map(candidate => {
        if (candidate.account.accountId === selectedCandidateId) {
          return {
            ...candidate,
            result: {
              ...candidate.result,
              writtenScore: newWrittenScore,
              totalScore: candidate.result.problemSolvingScore + 
                         newWrittenScore + mcqScore
            }
          };
        }
        return candidate;
      }));
  
      return {...prev, written: updatedWritten};
    });
  };
    const handleSaveAll = async () => {
        if (!editedSubmission || !examId || !selectedCandidateId) return;
        try {
            for (const problem of editedSubmission.problem) {
                await api.patch("/Review/Submission/Problem", {
                    examId: examId,
                    accountId: selectedCandidateId,
                    problemSubmissionId: problem.problemSubmissionId,
                    score: problem.score,
                    isFlagged: problem.isFlagged,
                    flagReason: problem.flagReason,
                });
            }
            for (const written of editedSubmission.written) {
                await api.patch("/Review/Submission/Written", {
                    examId: examId,
                    accountId: selectedCandidateId,
                    writtenSubmissionId: written.writtenSubmissionId,
                    score: written.score,
                    isFlagged: written.isFlagged,
                    flagReason: written.flagReason,
                });
            }
            toast.success("All changes saved successfully");
            const result = await api.get(
                `/Review/Candidates/${examId}/${selectedCandidateId}`
            );
            setMcqScore(result.data.submission.mcq.reduce((total:number, e:{score: number}) => total + e.score, 0));
            setEditedSubmission({
                problem: result.data.submission.problem,
                written: result.data.submission.written,
            });
        } catch (error) {
            console.error("Error saving submissions:", error);
            toast.error("Failed to save changes");
        }
    };
    const selectedCandidate = candidateList.find(
        (c) => c.account.accountId === selectedCandidateId
    );
    return (
        <div className="mx-44 flex flex-col justify-between">
            <h2 className="text-2xl font-bold text-center my-5">
                Review
            </h2>
            <div className="w-full px-12 -none flex flex-col gap-4">
                <Card className="space-y-4 p-5 bg-white dark:bg-[#18181b] shadow-none -none">
                    <h1 className="text-xl font-semibold w-full text-center">
                        Exam: {exam?.title}
                    </h1>
                    <div className="flex flex-col gap-3">
                        <div className="flex w-full items-center justify-between">
                            <div className="flex gap-2 items-center">
                                <span className="text-default-500">
                                    Candidate:
                                </span>
                                <Select
                                    aria-label="Select a candidate"
                                    className="w-80"
                                    value={selectedCandidateId}
                                    onChange={(e: {
                                        target: { value: string };
                                    }) => handleCandidateChange(e.target.value)}
                                >
                                    {candidateList?.map((candidate) => (
                                        <SelectItem
                                            key={candidate.account.accountId}
                                        >
                                            {candidate.account.username ??
                                                candidate.account.email}
                                        </SelectItem>
                                    ))}
                                </Select>
                            </div>
                            <div className="flex gap-2">
                                <Button size="sm"
                                    isDisabled={candidateList.findIndex((c) => c.account.accountId ===selectedCandidateId) <= 0}
                                    onPress={handlePrevCandidate} >
                                    Previous
                                </Button>
                                <Button
                                    size="sm"
                                    isDisabled={candidateList.findIndex((c) => c.account.accountId === selectedCandidateId) >=candidateList.length - 1
                                    }
                                    onPress={handleNextCandidate}>
                                    Next
                                </Button>
                            </div>
                        </div>
                        {selectedCandidate && (
                            <div>
                                <div className="flex items-center justify-between">
                                <div>
                                    <span className="text-default-500">Score:</span>
                                        {selectedCandidate.result.totalScore}/{totalPoints}
                                </div>
                                <div>
                                    <span className="text-default-500">Submitted At:</span>
                                    {new Date(selectedCandidate.result.submittedAt).toLocaleString()}
                                </div>
                            </div>
                            <div className='flex w-full justify-between'>
                                 <div>
                                    <span className="text-default-500">Problem Solving:</span>
                                        {selectedCandidate.result.problemSolvingScore}/{problemPoints}
                                </div>
                                 <div>
                                    <span className="text-default-500">Written Question:</span>
                                        {selectedCandidate.result.writtenScore}/{writtenPoints}
                                </div>
                                 <div>
                                    <span className="text-default-500">MCQ:</span>
                                        {mcqScore??0}/{mcqPoints}
                                </div>
                            </div>
                            </div>
                        )}
                    </div>
                </Card>
                {selectedCandidateId && editedSubmission && (
                    <div className="bg-white dark:bg-[#18181b] p-5 rounded-xl">
                        <h2 className="w-full text-center">
                            {selectedCandidate?.account.username ??
                                selectedCandidate?.account.email ??
                                "No Candidate Selected"}
                        </h2>
                        {editedSubmission.problem.length > 0 && (
                            <div className="mb-8">
                                <h2 className="text-lg font-semibold mb-4">
                                    Problem Solving Submissions
                                </h2>
                                {editedSubmission.problem.map((submission) => (
                                    <div key={submission.questionId}
                                        className="space-y-4 mb-6 p-4   rounded-lg">
                                        <h3 className="font-medium"> Problem Statement :</h3>
                                        <div className="font-medium">
                                            {questionsData[
                                                submission.questionId
                                            ] ? (
                                                <ReactMarkdown>
                                                { questionsData[ submission.questionId].statementMarkdown}
                                                </ReactMarkdown>
                                            ) : ("Loading question...")}
                                        </div>
                                        <div>
                                            <h4 className="font-semibold mb-2"> Code Submission: </h4>
                                            <Card className="p-4 rounded-lg bg-[#eeeef0] dark:bg-[#27272a] shadow-none">
                                                <div className="font-mono text-sm whitespace-pre-wrap p-2">
                                                    {submission.code}
                                                </div>
                                            </Card>
                                        </div>
                                       
                                        <div>
                                            <h4 className="font-semibold mb-2"> Result</h4>
                                            <div className="flex items-center gap-5">
                                                <div className="flex items-center gap-3">
                                                    <span>Points</span>
                                                    <input
                                                        type="number"
                                                        className="w-16"
                                                        value={submission.score}
                                                        onChange={(e) =>
                                                            updateProblemSubmission(
                                                                submission.questionId,
                                                                { score: parseInt( e.target.value )} ) }
                                                    /> /{questionsData[submission.questionId].points}
                                                </div>
                                                <Button
                                                    size="sm"
                                                    variant="flat"
                                                >
                                                    <input
                                                        type="checkbox"
                                                        checked={
                                                            submission.isFlagged
                                                        }
                                                        onChange={(e) =>
                                                            updateProblemSubmission(
                                                                submission.questionId,
                                                                {
                                                                    isFlagged:e.target.checked,
                                                                }
                                                            )
                                                        }
                                                    />
                                                    Flag Solution
                                                </Button>
                                            </div>
                                        </div>
                                        {submission.isFlagged && (
                                            <Textarea
                                                className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
                                                placeholder="Flag reason"
                                                value={submission.flagReason || ""}
                                                onChange={(e) =>
                                                    updateProblemSubmission(
                                                        submission.questionId,{flagReason:e.target.value})
                                                }/>)}
                                    </div>
                                ))}
                            </div>
                        )}
                        {editedSubmission.written.length > 0 && (
                            <div className="mb-8">
                                <h2 className="text-lg font-semibold mb-4">
                                    Written Submissions
                                </h2>
                                {editedSubmission.written.map((submission) => (
                                    <div key={submission.questionId}
                                        className="space-y-4 p-4  rounded-lg">
                                        <div  className="space-y-4  p-4  rounded-lg">
                                            <div className="font-medium">
                                                {questionsData[submission.questionId] ? (
                                                    <ReactMarkdown>{questionsData[submission.questionId].statementMarkdown}</ReactMarkdown>
                                                ) : (
                                                    'Loading question...'
                                                )}
                                            </div>
                                            <div>
                                                <Card className="p-4 rounded-lg bg-[#eeeef0] dark:bg-[#27272a] shadow-none">
                                                    <div className="whitespace-pre-wrap p-2">
                                                    {submission.answer}
                                                </div>
                                            </Card>
                                        </div>
                                        <div>
                                            <h4 className="font-semibold mb-2">Result</h4>
                                            <div className="flex items-center gap-5">
                                                <div className="flex items-center gap-3">
                                                    <span>Points</span>
                                                    <input
                                                        type="number"
                                                        className="w-16"
                                                        value={submission.score}
                                                        onChange={(e) =>
                                                            updateWrittenSubmission(
                                                                submission.questionId,
                                                                {score: parseInt(e.target.value)}
                                                            )
                                                        }
                                                    />
                                                    /{questionsData[submission.questionId].score}
                                                </div>
                                                <Button
                                                    size="sm"
                                                    variant="flat"
                                                >
                                                    <input
                                                        type="checkbox"
                                                        checked={
                                                            submission.isFlagged
                                                        }
                                                        onChange={(e) =>
                                                            updateWrittenSubmission(
                                                                submission.questionId,
                                                                {isFlagged:e.target.checked})}
                                                    />
                                                    Flag Solution
                                                </Button>
                                            </div>
                                        </div>
                                        {submission.isFlagged && (
                                            <Textarea
                                                className="bg-[#eeeef0] dark:bg-[#27272a] rounded-xl"
                                                placeholder="Flag reason"
                                                value={submission.flagReason || ""}
                                                onChange={(e) =>
                                                    updateWrittenSubmission(
                                                        submission.questionId,{flagReason:e.target.value,}
                                                    )
                                                }
                                            />
                                        )}
                                    </div>
                                    </div>
                                ))}
                            </div>
                        )}
                        <div className="w-full flex justify-center">
                            <Button
                                className="my-3"
                                color="primary"
                                onPress={handleSaveAll}
                            >
                                Save All Changes
                            </Button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}
