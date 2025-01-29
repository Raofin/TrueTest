using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Usr;

namespace OPS.Domain.Entities.Exam;

public partial class Question
{
    public long QuestionId { get; set; }
    public string Statement { get; set; } = null!;
    public decimal Score { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public long QuestionTypeId { get; set; }
    public long ExamId { get; set; }

    public Examination Exam { get; set; } = null!;
    public QuestionType QuestionType { get; set; } = null!;
    public ICollection<Problem> Problems { get; set; } = [];
    public ICollection<UserMcqAnswer> UserMcqAnswers { get; set; } = [];
    public ICollection<UserWrittenAnswer> UserWrittenAnswers { get; set; } = [];
    public ICollection<McqOption> McqOptions { get; set; } = [];
}
