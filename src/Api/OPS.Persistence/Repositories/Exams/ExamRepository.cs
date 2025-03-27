using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Exams;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Exams;

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

    public async Task<Examination?> GetWithQuestionsAsync(Guid examId, CancellationToken cancellationToken)
    {
        return await _dbContext.Examinations
            .AsNoTracking()
            .Where(e => e.Id == examId)
            .Include(e => e.Questions).ThenInclude(q => q.TestCases)
            .Include(e => e.Questions).ThenInclude(q => q.McqOption)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Examination?> GetWithAllQuesAndSubmission(
        Guid examId, Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Examinations
            .Where(e => e.Id == examId)
            .Include(e => e.Questions).ThenInclude(q => q.McqOption)
            .Include(e => e.Questions).ThenInclude(q => q.McqSubmissions.Where(s => s.AccountId == accountId))
            .Include(e => e.Questions).ThenInclude(q => q.TestCases).ThenInclude(tc => tc.TestCaseOutputs)
            .Include(e => e.Questions).ThenInclude(q => q.ProblemSubmissions.Where(ps => ps.AccountId == accountId))
            .Include(e => e.Questions).ThenInclude(q => q.WrittenSubmissions.Where(s => s.AccountId == accountId))
            .FirstOrDefaultAsync(cancellationToken);
    }
}