using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Enums;

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

    public async Task<WrittenSubmission?> GetByAccountIdAsync(Guid questionId, Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.WrittenSubmissions
            .Where(s => s.QuestionId == questionId && s.AccountId == accountId)
            .SingleOrDefaultAsync(cancellationToken);
    }
    
    public async Task<List<Question>> GetQuesWithSubmission(Guid examId, Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Where(q => q.ExaminationId == examId && q.QuestionTypeId == (int)QuestionType.Written)
            .Include(q => q.WrittenSubmissions.Where(s => s.AccountId == accountId))
            .ToListAsync(cancellationToken);
    }
}