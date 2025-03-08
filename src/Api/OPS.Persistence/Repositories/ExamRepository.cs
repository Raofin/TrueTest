using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Repositories;

internal class ExamRepository(AppDbContext dbContext) : Repository<Examination>(dbContext), IExamRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Examination>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Examinations
            .AsNoTracking()
            .Where(e => e.ExamCandidates.Any(candidate => candidate.AccountId == accountId))
            .OrderBy(exam => exam.OpensAt)
            .ToListAsync(cancellationToken);
    }
}