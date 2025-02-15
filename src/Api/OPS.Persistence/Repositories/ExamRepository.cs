using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Repositories;

internal class ExamRepository(AppDbContext dbContext) : Repository<Examination>(dbContext), IExamRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Examination>> GetAllExamsByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Exams
             .AsNoTracking()
             .Where(exam => _dbContext.ExamCandidates .Any(ec => ec.ExaminationId == exam.Id && ec.AccountId == accountId))
             .OrderBy(exam => exam.OpensAt)
             .ToListAsync(cancellationToken);
    }

    public async Task<List<Examination>> GetPreviousExamsByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Exams
             .AsNoTracking()
             .Where(ec => ec.ClosesAt  < DateTime.UtcNow)
             .Where(exam => _dbContext.ExamCandidates
             .Any(ec => ec.ExaminationId == exam.Id && ec.AccountId == accountId))
             .OrderBy(exam => exam.OpensAt)
             .ToListAsync(cancellationToken);
    }

    public async Task<List<Examination>> GetUpcomingExamsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Exams
            .AsNoTracking()
            .Where(exam => exam.OpensAt > DateTime.UtcNow)
            .OrderBy(exam => exam.OpensAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Examination>> GetUpcomingExamsByAccountIdAsync(Guid accountId
        , CancellationToken cancellationToken)
    {
        return await _dbContext.Exams
             .AsNoTracking()
             .Where(exam => exam.OpensAt > DateTime.UtcNow)
             .Where(exam => _dbContext.ExamCandidates
             .Any(ec => ec.ExaminationId == exam.Id && ec.AccountId == accountId))
             .OrderBy(exam => exam.OpensAt)
             .ToListAsync(cancellationToken);
    }
}