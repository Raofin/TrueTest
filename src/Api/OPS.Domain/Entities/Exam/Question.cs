using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Entities.Exam;

public class Question
{
    public long QuestionId { get; set; }
    public string StatementMarkdown { get; set; } = null!;
    public decimal Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public long ExaminationId { get; set; }
    public long DifficultyId { get; set; }
    public long QuestionTypeId { get; set; }
    public Examination Examination { get; set; } = null!;
    public Difficulty Difficulty { get; set; } = null!;
    public QuestionType QuestionType { get; set; } = null!;

    public ICollection<TestCase> TestCases { get; set; } = [];
    public ICollection<McqAnswer> McqAnswers { get; set; } = [];
    public ICollection<McqOption> McqQptions { get; set; } = [];
    public ICollection<McqSubmission> McqSubmissions { get; set; } = [];
    public ICollection<WrittenSubmission> WrittenSubmissions { get; set; } = [];
    public ICollection<ProblemSubmission> ProblemSubmissions { get; set; } = [];
}