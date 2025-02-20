using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Repositories;

internal class WrittenSubmissionRepository(AppDbContext dbContext) : Repository<WrittenSubmission>(dbContext), IWrittenSubmissionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<WrittenSubmission>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.WrittenSubmissions
            .AsNoTracking()
            .Where(q => q.QuestionId == questionId)
            .ToListAsync(cancellationToken);
    }
}