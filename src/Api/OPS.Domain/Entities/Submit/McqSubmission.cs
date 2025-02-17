using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Entities.Submit;

public class McqSubmission : BaseEntity
{
    public string AnswerOptions { get; set; } = null!;

    public Guid AccountId { get; set; }
    public Guid McqOptionId { get; set; }
    public Account Account { get; set; } = null!;
    public McqOption McqOption { get; set; } = null!;
}