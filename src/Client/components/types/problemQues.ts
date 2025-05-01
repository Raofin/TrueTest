export interface TestCase {
  testCaseId?: string;
  input: string;
  output: string;
  receivedOutput?: string;
  status?: "success" | "error" | "pending";
  executionTime:number,
  errorMessage:string
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
export interface Problem {
  questionId?: string;
  question: string;
  testCases: {
    input: string;
    output: string;
  }[];
  points: number;
  difficultyType?: string;
}
export interface TestCaseItemProps {
    testCaseIndex: number;
    testCase:  {
      input: string;
      output: string;
    };
    onDelete: (testCaseIndex: number) => void;
    onRefresh: (testCaseIndex: number) => void;
    onInputChange: (field: "input" | "output", value: string) => void;
}
export interface FormFooterProps {
    totalPages: number;
    currentPage: number;
    onPrevious: () => void;
    onNext: () => void;
}
export interface ProblemSolvingFormProps {
  readonly examId: string;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  readonly existingQuestions: any[];
  readonly onSaved: () => void;
  readonly problemPoints: (points: number) => void;
}
export interface ProblemItemProps {
    problem: Problem;
    onDeleteTestCase: (testCaseIndex: number) => void;
    onRefreshTestCase: (testCaseIndex: number) => void;
    onTestCaseChange: (
        testCaseIndex: number,
        field: "input" | "output",
        value: string
    ) => void;
    onQuestionChange: (value: string) => void;
    onPointsChange: (value: number) => void;
    onAddTestCase: () => void;
    onDifficultyChange: (value: string) => void;
    onDeleteProblem: () => void;
}