using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Contracts.Repository.Submissions;

public interface IMcqSubmissionRepository : IBaseRepository<McqSubmission>
{
    Task<McqSubmission?> GetByAccountIdAsync(Guid questionId, Guid accountId, CancellationToken cancellationToken);
    Task<List<Question>> GetMcqQuesWithSubmission(Guid examId, Guid accountId, CancellationToken cancellationToken);
}