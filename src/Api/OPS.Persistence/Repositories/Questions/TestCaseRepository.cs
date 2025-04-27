using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Questions;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Questions;

internal class TestCaseRepository(AppDbContext dbContext) : Repository<TestCase>(dbContext), ITestCaseRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<TestCase>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.TestCases
            .AsNoTracking()
            .Where(q => q.QuestionId == questionId)
            .OrderBy(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}