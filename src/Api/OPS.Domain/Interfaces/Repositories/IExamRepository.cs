using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Interfaces.Repositories;

public interface IExamRepository : IRepository<Examination>
{
    Task<List<Examination>> GetUpcomingExamsAsync();
}