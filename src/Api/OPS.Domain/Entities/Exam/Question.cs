﻿using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Entities.Exam;

public class Question : SoftDeletableEntity
{
    public string StatementMarkdown { get; set; } = null!;
    public decimal Points { get; set; }
    public bool HasLongAnswer { get; set; }

    public int QuestionTypeId { get; set; }
    public int DifficultyId { get; set; }
    public Guid ExaminationId { get; set; }
    public Examination Examination { get; set; } = null!;
    public Difficulty Difficulty { get; set; } = null!;
    public QuestionType QuestionType { get; set; } = null!;

    public McqOption? McqOption { get; set; }
    public ICollection<TestCase> TestCases { get; set; } = [];
    public ICollection<WrittenSubmission> WrittenSubmissions { get; set; } = [];
    public ICollection<McqSubmission> McqSubmissions { get; set; } = [];
    public ICollection<ProblemSubmission> ProblemSubmissions { get; set; } = [];
}