using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts;

public interface IExamRepository : IRepository<Examination>
{
    Task<List<Examination>> GetUpcomingExamsAsync(CancellationToken cancellationToken);
}