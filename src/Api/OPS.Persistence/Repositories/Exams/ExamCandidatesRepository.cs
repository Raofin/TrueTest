using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Exams;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Exams;

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
