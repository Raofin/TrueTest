namespace OPS.Application.Dtos;

public record ExamResponse(
    Guid ExamId,
    string Title,
    string Description,
    decimal TotalPoints,
    decimal ProblemSolvingPoints,
    decimal WrittenPoints,
    decimal McqPoints,
    int DurationMinutes,
    string Status,
    DateTime OpensAt,
    DateTime ClosesAt
);

public record ExamWithQuestionsResponse(
    ExamResponse Exam,
    QuestionResponses Questions
);

public record QuestionResponses(
    List<ProblemQuestionResponse> Problem,
    List<WrittenQuestionResponse> Written,
    List<McqQuestionResponse> Mcq
);

public record ExamQuesWithSubmissionResponse(
    ExamResponse Exam,
    AccountBasicInfoResponse Account,
    ResultResponse? Result,
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

public record ResultResponse(
    decimal TotalScore,
    decimal ProblemSolvingScore,
    decimal WrittenScore,
    decimal McqScore,
    DateTime? StartedAt,
    DateTime? SubmittedAt,
    bool HasCheated,
    bool IsReviewed
);