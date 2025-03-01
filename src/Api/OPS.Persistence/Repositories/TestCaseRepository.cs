using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Repositories;

internal class TestCaseRepository(AppDbContext dbContext) : Repository<TestCase>(dbContext), ITestCaseRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<TestCase>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.TestCases
           .AsNoTracking()
           .Where(q => q.QuestionId == questionId)
           .OrderBy(exam => exam.CreatedAt)
           .ToListAsync(cancellationToken);
    }
}
