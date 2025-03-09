using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Repositories;

internal class ProblemSubmissionRepository(AppDbContext dbContext) : Repository<ProblemSubmission>(dbContext), IProblemSubmissionRepository
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
}