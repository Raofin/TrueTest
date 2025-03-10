using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Contracts;

public interface IMcqSubmissionRepository : IBaseRepository<McqSubmission>
{
    Task<McqSubmission?> GetByAccountIdAsync(Guid questionId, Guid accountId, CancellationToken cancellationToken);
    Task<List<Question>> GetMcqQuesWithSubmission(Guid examId, Guid accountId, CancellationToken cancellationToken);
}