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
export interface TestCaseResults {
    [questionId: string]: TestCase[];
}
