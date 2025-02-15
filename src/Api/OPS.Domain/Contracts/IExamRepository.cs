using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts;

public interface IExamRepository : IRepository<Examination>
{
    Task<List<Examination>> GetUpcomingExamsAsync(CancellationToken cancellationToken);
    Task<List<Examination>> GetAllExamsByAccountIdAsync(Guid id,CancellationToken cancellationToken);

    Task<List<Examination>> GetPreviousExamsByAccountIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Examination>> GetUpcomingExamsByAccountIdAsync(Guid id, CancellationToken cancellationToken);

}