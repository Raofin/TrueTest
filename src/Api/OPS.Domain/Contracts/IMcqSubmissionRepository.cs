using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Contracts;

public interface IMcqSubmissionRepository : IBaseRepository<McqSubmission>
{
    Task<List<McqSubmission>> GetMcqSubmissionsByMcqOptionIdAsync(Guid mcqOptionId, CancellationToken cancellationToken);
}