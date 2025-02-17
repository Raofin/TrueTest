using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Contracts;

public interface IWrittenSubmissionRepository : IBaseRepository<WrittenSubmission>
{
    Task<List<WrittenSubmission>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken);
}