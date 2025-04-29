import { McqQuestion } from './mcqQues'
import { ProblemQuestion } from './problemQues'
import { WrittenQuestion } from './writtenQues'

export interface ExamResponse {
    exam: ExamData;
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
export interface Exam {
    exam: {
        examId: string;
        title: string;
        totalPoints: number;
        durationMinutes: number;
        problemSolvingPoints: number;
        status: string;
        writtenPoints: number;
        mcqPoints: number;
        opensAt: string;
        closesAt: string;
    };
}
export interface ExamData {
  examId: string;
  title: string;
  description: string;
  totalPoints: number;
  problemSolvingPoints: number;
  writtenPoints: number;
  mcqPoints: number;
  durationMinutes: number;
  isPublished: boolean;
  status: string;
  opensAt: string;
  closesAt: string;
}
export interface viewExam {
  examId: string;
  title: string;
  description: string;
  durationMinutes: number;
  opensAt: string;
  createdAt: string;
  closesAt: string;
  totalPoints: number;
  problemSolvingPoints: number;
  writtenPoints: number;
  mcqPoints: number;
  status: "Running" | "Scheduled" | "Ended";
  acceptedCandidates: number;
  problemSolving: number;
  written: number;
  mcq: number;
  isPublished: boolean;
}
export interface Questions {
  problem: ProblemQuestion[];
  written: WrittenQuestion[];
  mcq: McqQuestion[];
}
export interface QuestionData {
    questions: Questions;
    submits: {
        problem: [];
        written: [];
        mcq: [];
    };
}
