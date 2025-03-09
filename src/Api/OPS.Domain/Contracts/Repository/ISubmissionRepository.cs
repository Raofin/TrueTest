using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts.Repository;

public interface ISubmissionRepository : IBaseRepository<Question>
{
    Task<Question?> GetWithProblemSubmissionsAsync(Guid questionId, Guid accountId, CancellationToken cancellationToken);
    
}