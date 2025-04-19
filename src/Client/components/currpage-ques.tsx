'use client'

export interface TestCase {
  testCaseId?: string;
  input: string;
  output: string;  
  receivedOutput?: string;
  status?: 'success' | 'error' | 'pending';
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


interface WrittenQuestion {
    questionId: string;
    examId: string;
    questionType: string;
    hasLongAnswer: boolean;
    statementMarkdown: string;
    score: number;
    difficultyType: string;
}

interface MCQOption {
    option1: string;
    option2: string;
    option3: string;
    option4: string;
    isMultiSelect: boolean;
    answerOptions: string;
}

interface MCQQuestion {
    questionId: string;
    examId: string;
    questionType: string;
    statementMarkdown: string;
    score: number;
    difficultyType: string;
    mcqOption: MCQOption;
}

interface Questions {
    problem: ProblemQuestion[];
    written: WrittenQuestion[];
    mcq: MCQQuestion[];
}

interface QuestionData {
    questions: Questions;
    submits: {
        problem: [];
        written: [];
        mcq: [];
    };
}
interface PageProps {
  currentPage: number;
  questions: QuestionData;
  perpageques: number;
}

export default function getQuestionsForCurrentPage({ currentPage, questions, perpageques }: PageProps) {
  const allQuestions = [
    ...questions.questions.problem,
    ...questions.questions.written,
    ...questions.questions.mcq
  ];
  console.log(allQuestions)

  const startIndex = (currentPage - 1) * perpageques;
  const endIndex = startIndex + perpageques;
  console.log(startIndex,endIndex)
  return allQuestions.slice(startIndex, endIndex);
}