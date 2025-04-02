using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Exams;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Exams;

internal class ExamCandidatesRepository(AppDbContext dbContext)
    : Repository<ExamCandidate>(dbContext), IExamCandidatesRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<ExamCandidate>> GetExamParticipantsAsync(Guid examId, CancellationToken cancellationToken)
    {
        return await _dbContext.ExamCandidates
            .AsNoTracking()
            .Where(ec => ec.ExaminationId == examId && ec.StartedAt != null)
            .Include(ec => ec.Account)
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