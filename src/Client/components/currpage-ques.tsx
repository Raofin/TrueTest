'use client'

interface Question {
  id: number
  title: string
  description: string
  questionTypeId: number 
  options?: string[]
  points: number
}

interface PageProps{
  currentPage:number;
  questions:Question[];
  perpageques:number
}
export default function getQuestionsForCurrentPage ({currentPage,questions,perpageques}:PageProps){
    const regularQuestions = questions.filter((q) => q.questionTypeId !== 3)
    const codingQuestions = questions.filter((q) => q.questionTypeId === 3)
    const regularQuestionsPages = Math.ceil(regularQuestions.length / perpageques)
    if (currentPage <= regularQuestionsPages) {
      const startIndex = (currentPage - 1) * perpageques
      return regularQuestions.slice(startIndex, startIndex + perpageques)
    }
    const codingIndex = currentPage - regularQuestionsPages - 1
    return codingIndex >= 0 && codingIndex < codingQuestions.length ? [codingQuestions[codingIndex]] : []
  }
