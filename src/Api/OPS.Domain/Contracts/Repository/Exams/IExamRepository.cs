using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts.Repository.Exams;

public interface IExamRepository : IBaseRepository<Examination>
{
    Task<List<Examination>> GetByAccountIdAsync(Guid id,CancellationToken cancellationToken);
}