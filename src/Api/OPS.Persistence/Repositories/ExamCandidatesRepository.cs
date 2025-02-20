using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Repositories;

internal class ExamCandidatesRepository(AppDbContext dbContext) : Repository<ExamCandidate>(dbContext), IExamCandidatesRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<ExamCandidate>> GetExamCandidateAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.ExamCandidates
             .AsNoTracking()
             .ToListAsync(cancellationToken);
    }

    public async Task<List<ExamCandidate>> GetExamCandidateByAccountAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.ExamCandidates
             .AsNoTracking()
             .Where(ec => ec.AccountId == id)
             .ToListAsync(cancellationToken);
    }
}
