using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Repositories;

internal class ExamRepository(AppDbContext dbContext) : Repository<Examination>(dbContext), IExamRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Examination>> GetUpcomingExamsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Examinations
            .AsNoTracking()
            .Where(exam => exam.OpensAt > DateTime.UtcNow)
            .OrderBy(exam => exam.OpensAt)
            .ToListAsync(cancellationToken);
    }
}