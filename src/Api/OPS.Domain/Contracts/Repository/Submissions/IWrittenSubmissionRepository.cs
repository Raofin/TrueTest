using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Contracts.Repository.Submissions;

public interface IWrittenSubmissionRepository : IBaseRepository<WrittenSubmission>
{
    Task<List<WrittenSubmission>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken);
    Task<WrittenSubmission?> GetByAccountIdAsync(Guid questionId, Guid accountId, CancellationToken cancellationToken);
    Task<List<Question>> GetQuesWithSubmission(Guid examId, Guid accountId, CancellationToken cancellationToken);
}