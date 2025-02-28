using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Exam;

namespace OPS.Persistence.Repositories;

internal class QuestionRepository(AppDbContext dbContext) : Repository<Question>(dbContext), IQuestionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Question>> GetAllQuestionByExamIdAsync(Guid examId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
           .AsNoTracking()
           .Where(q => q.ExaminationId == examId)
           .OrderBy(exam => exam.CreatedAt)
           .ToListAsync(cancellationToken);
    }

    public async Task<List<Question>> GetAllQuestionByExamIdQuestionTypeIdAsync(Guid examId,int questionTypeId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
           .AsNoTracking()
           .Where(q => q.ExaminationId == examId && q.QuestionTypeId == questionTypeId)
           .OrderBy(exam => exam.CreatedAt)
           .ToListAsync(cancellationToken);
    }
}