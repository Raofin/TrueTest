namespace OPS.Application.Dtos;

public record ExamResponse(
    Guid ExamId,
    string Title,
    string Description,
    int DurationMinutes,
    string Status,
    DateTime OpensAt,
    DateTime ClosesAt
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

public record ExamReviewResponse(
    Guid ExamId,
    string Title,
    int DurationMinutes,
    int TotalPoints,
    AccountResponse Account,
    ExamResultsResponse Results,
    QuestionsWithSubmissionResponse QuestionsWithSubmission
);

public record ExamResultsResponse(
    decimal Score,
    DateTime? StartedAt,
    DateTime? SubmittedAt,
    bool HasCheated
);

public record QuestionsWithSubmissionResponse(
    List<ProblemQuesWithSubmissionResponse?> Problem,
    List<WrittenQuesWithSubmissionResponse?> Written,
    List<McqQuesWithSubmissionResponse?> Mcq
);

public record ExamStartResponse(
    Guid ExamId,
    DateTime StartedAt,
    DateTime ClosesAt,
    QuestionResponses Questions,
    SubmitResponse Submits
);

public record QuestionsWithSubmitsResponse(
    List<ProblemQuesWithSubmitResponse?> Problem,
    List<WrittenQuesWithSubmitResponse?> Written,
    List<McqQuesWithSubmitResponse?> Mcq
);

public record SubmitResponse(
    List<ProblemSubmitResponse?> Problem,
    List<WrittenSubmitResponse?> Written,
    List<McqSubmitResponse?> Mcq
);