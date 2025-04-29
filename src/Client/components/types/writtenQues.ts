export interface WrittenQuestion {
  questionId: string;
  examId: string;
  questionType: string;
  hasLongAnswer: boolean;
  statementMarkdown: string;
  score: number;
  difficultyType: string;
}