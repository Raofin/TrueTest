using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Repositories;

internal class McqSubmissionRepository(AppDbContext dbContext) : Repository<McqSubmission>(dbContext), IMcqSubmissionRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    public async Task<List<McqSubmission>> GetMcqSubmissionsByMcqOptionIdAsync(Guid mcqOptionId, CancellationToken cancellationToken)
    {
        return await _dbContext.McqSubmissions
            .AsNoTracking()
            .Where(submission => submission.McqOptionId == mcqOptionId)
            .OrderBy(submission => submission.CreatedAt)
            .ToListAsync(cancellationToken);    
    }
}