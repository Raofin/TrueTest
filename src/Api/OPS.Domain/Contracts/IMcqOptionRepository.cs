using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Contracts;

public interface IMcqOptionRepository : IBaseRepository<McqOption>
{
    Task<List<McqOption>> GetMcqOptionsByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken);
}