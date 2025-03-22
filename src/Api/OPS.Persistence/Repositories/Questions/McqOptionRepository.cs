using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Questions;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Questions;

internal class McqOptionRepository(AppDbContext dbContext) : Repository<McqOption>(dbContext), IMcqOptionRepository
{
    private readonly AppDbContext _dbContext = dbContext;
   
    public async Task<List<McqOption>> GetMcqOptionsByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.McqOption
            .AsNoTracking()
            .Where(option => option.QuestionId == questionId)
            .OrderBy(option => option.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}