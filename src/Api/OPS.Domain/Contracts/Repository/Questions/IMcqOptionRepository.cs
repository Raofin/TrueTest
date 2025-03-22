using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts.Repository.Questions;

public interface IMcqOptionRepository : IBaseRepository<McqOption>
{
    Task<List<McqOption>> GetMcqOptionsByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken);
}