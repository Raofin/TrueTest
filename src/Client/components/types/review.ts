import { ExamData } from './exam'

export interface TestCase {
    input: string;
    expectedOutput: string;
    receivedOutput: string;
}
export interface CandidateData {
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
        submittedAt: string;
        hasCheated: boolean;
        isReviewed: boolean;
    };
}
export interface ProblemSubmission {
    questionId: string;
    problemSubmissionId: string;
    code: string;
    language: string;
    attempts: number;
    score: number;
    isFlagged: boolean;
    flagReason: string | null;
    testCaseOutputs: TestCase[];
}
export interface CandidatesResponse {
    exam: ExamData;
    candidates: CandidateData[];
  }
export interface WrittenSubmission {
    questionId: string;
    writtenSubmissionId: string;
    answer: string;
    score: number;
    isFlagged: boolean;
    flagReason: string | null;
}
export interface CandidateSubmission {
    problem: ProblemSubmission[];
    written: WrittenSubmission[];
}

export interface ExamResponse {
    exam: ExamData;
    questions: {
        problem: ProblemSubmission[];
        written: WrittenSubmission[];
    };
}
export interface AiApiResponse{
     review:string;
     score:number
}
