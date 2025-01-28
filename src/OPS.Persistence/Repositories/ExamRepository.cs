using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Repositories;

namespace OPS.Persistence.Repositories;

internal class ExamRepository(AppDbContext dbContext) : Repository<Examination>(dbContext), IExamRepository
{
    private readonly AppDbContext _dbContext = dbContext;


}
