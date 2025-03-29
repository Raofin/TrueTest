using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Submissions;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Submissions;

internal class ProblemSubmissionRepository(AppDbContext dbContext)
    : Repository<ProblemSubmission>(dbContext), IProblemSubmissionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<ProblemSubmission?> GetWithOutputsAsync(
        Guid questionId, Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProblemSubmissions
            .Include(ps => ps.TestCaseOutputs)
            .Where(ps => ps.QuestionId == questionId && ps.AccountId == accountId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ProblemSubmission?> GetWithOutputsAsync(
        Guid problemSubmissionId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProblemSubmissions
            .Include(ps => ps.TestCaseOutputs)
            .Where(ps => ps.Id == problemSubmissionId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Question>> GetAllProblemsWithSubmission(
        Guid examId, Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Where(q => q.ExaminationId == examId && q.QuestionTypeId == (int)QuestionType.ProblemSolving)
            .Include(q => q.TestCases)
            .ThenInclude(tc => tc.TestCaseOutputs)
            .Include(q => q.ProblemSubmissions.Where(ps => ps.AccountId == accountId))
            .OrderBy(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ProblemSubmission>> GetAllAsync(
        Guid examId, Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProblemSubmissions
            .Where(ps => ps.AccountId == accountId && ps.Question.ExaminationId == examId)
            .Include(ps => ps.TestCaseOutputs)
            .ToListAsync(cancellationToken);
    }
}