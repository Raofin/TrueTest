using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Entities.Submit;

public class McqSubmission : BaseEntity
{
    public string AnswerOptions { get; set; } = null!;
    public decimal Score { get; set; }

    public Guid AccountId { get; set; }
    public Guid McqOptionId { get; set; }
    public Guid QuestionId { get; set; }
    public Account Account { get; set; } = null!;
    public McqOption McqOption { get; set; } = null!;
    public Question Question { get; set; } = null!;
}