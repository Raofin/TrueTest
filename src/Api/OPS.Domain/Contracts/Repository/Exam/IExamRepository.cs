using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts.Repository;

public interface IExamRepository : IBaseRepository<Examination>
{
    Task<List<Examination>> GetByAccountIdAsync(Guid id,CancellationToken cancellationToken);
}