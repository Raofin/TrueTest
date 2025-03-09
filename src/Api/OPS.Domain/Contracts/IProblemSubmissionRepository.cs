using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Contracts;

public interface IProblemSubmissionRepository : IBaseRepository<ProblemSubmission>
{
    Task<ProblemSubmission?> GetWithOutputsAsync(Guid questionId, Guid accountId, CancellationToken cancellationToken);
}