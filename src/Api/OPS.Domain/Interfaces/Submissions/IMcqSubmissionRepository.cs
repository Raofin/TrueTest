using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Submissions;

public interface IMcqSubmissionRepository : IBaseRepository<McqSubmission>
{
    Task<McqSubmission?> GetByAccountIdAsync(Guid questionId, Guid accountId, CancellationToken cancellationToken);
    Task<List<Question>> GetMcqQuesWithSubmission(Guid examId, Guid accountId, CancellationToken cancellationToken);
    Task<List<McqSubmission>> GetAllAsync(Guid examId, Guid accountId, CancellationToken cancellationToken);
}