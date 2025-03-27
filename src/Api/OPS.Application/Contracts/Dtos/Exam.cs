namespace OPS.Application.Contracts.Dtos;

public record ExamResponse(
    Guid ExamId,
    string Title,
    string Description,
    int DurationMinutes,
    string Status,
    DateTime OpensAt,
    DateTime ClosesAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool IsActive
);

public record ExamWithQuestionsResponse(
    Guid ExamId,
    string Title,
    string Description,
    int DurationMinutes,
    string Status,
    DateTime OpensAt,
    DateTime ClosesAt,
    QuestionResponses Questions
);

public record QuestionResponses(
    List<ProblemQuestionResponse> Problem,
    List<WrittenQuestionResponse> Written,
    List<McqQuestionResponse> Mcq
);

public record OngoingExamResponse(
    Guid ExamId,
    string Title,
    string Description,
    int DurationMinutes,
    string Status,
    DateTime OpensAt,
    DateTime ClosesAt,
    QuestionsWithSubmission QuestionsWithSubmission
);

public record QuestionsWithSubmission(
    List<ProblemQuesWithSubmissionResponse> Problem,
    List<WrittenQuesWithSubmissionResponse> Written,
    List<McqQuesWithSubmissionResponse> Mcq
);