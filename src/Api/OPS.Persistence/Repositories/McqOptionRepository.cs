using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Contracts;

namespace OPS.Persistence.Repositories;

internal class McqOptionRepository(AppDbContext dbContext) : Repository<McqOption>(dbContext), IMcqOptionRepository
{
    private readonly AppDbContext _dbContext = dbContext;
   
    public async Task<List<McqOption>> GetMcqOptionsByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.McqOptions
            .AsNoTracking()
            .Where(option => option.QuestionId == questionId)
            .OrderBy(option => option.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}