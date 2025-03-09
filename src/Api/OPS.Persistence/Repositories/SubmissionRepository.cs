using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Enums;

namespace OPS.Persistence.Repositories;

internal class SubmissionRepository(AppDbContext dbContext) : Repository<Question>(dbContext), ISubmissionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Question?> GetWithProblemSubmissionsAsync(Guid questionId, Guid accountId, CancellationToken cancellationToken)
    {
        var hasSubmissions = await _dbContext.ProblemSubmissions
            .AnyAsync(ps => ps.AccountId == accountId && ps.QuestionId == questionId, cancellationToken);

        if (hasSubmissions)
        {
            return await _dbContext.Questions
                .Include(q => q.ProblemSubmissions)
                .Include(q => q.TestCases)
                .ThenInclude(q => q.TestCaseOutputs)
                .Where(q => q.Id == questionId 
                            && q.QuestionTypeId == (int)QuestionType.ProblemSolving 
                            && q.ProblemSubmissions.Any(ps => ps.AccountId == accountId))
                .SingleOrDefaultAsync(cancellationToken);
        }
        
        return await _dbContext.Questions
            .Include(q => q.TestCases)
            .Where(q => q.Id == questionId && q.QuestionTypeId == (int)QuestionType.ProblemSolving)
            .SingleOrDefaultAsync(cancellationToken);
    }
}