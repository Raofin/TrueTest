using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Repositories;

internal class WrittenSubmissionRepository(AppDbContext dbContext) : Repository<WrittenSubmission>(dbContext), IWrittenSubmissionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<WrittenSubmission>> GetAllWrittenSubmissionByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.UserWrittenAnswers
           .AsNoTracking()
           .Where(q => q.QuestionId == questionId)
           .ToListAsync(cancellationToken);
    }
}