using Microsoft.EntityFrameworkCore;
using OPS.Persistence.Repositories.Common;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Questions;
using QuestionType = OPS.Domain.Enums.QuestionType;

namespace OPS.Persistence.Repositories.Questions;

internal class QuestionRepository(AppDbContext dbContext) : Repository<Question>(dbContext), IQuestionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Question?> GetWithTestCases(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Include(q => q.Examination)
            .Include(q => q.TestCases)
            .Where(q => q.Id == questionId)
            .OrderBy(q => q.CreatedAt)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Question>> GetProblemSolvingByExamIdAsync(Guid examId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .AsNoTracking()
            .Include(q => q.TestCases)
            .Where(q => q.ExaminationId == examId && q.QuestionTypeId == (int)QuestionType.ProblemSolving)
            .OrderBy(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Question?> GetWithMcqOption(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Include(q => q.Examination)
            .Include(q => q.McqOption)
            .Where(q => q.Id == questionId)
            .OrderBy(q => q.CreatedAt)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Question>> GetMcqByExamIdAsync(Guid examId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .AsNoTracking()
            .Include(q => q.McqOption)
            .Where(q => q.ExaminationId == examId && q.QuestionTypeId == (int)QuestionType.MCQ)
            .OrderBy(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Question?> GetWrittenByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Where(q => q.Id == questionId && q.QuestionTypeId == (int)QuestionType.Written)
            .OrderBy(q => q.CreatedAt)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Question?> GetWithExamAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Include(q => q.Examination)
            .Where(q => q.Id == questionId)
            .OrderBy(q => q.CreatedAt)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Question>> GetWrittenByExamIdAsync(Guid examId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Where(q => q.ExaminationId == examId && q.QuestionTypeId == (int)QuestionType.Written)
            .OrderBy(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetPointsAsync(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .AsNoTracking()
            .Where(q => q.Id == questionId)
            .Select(q => q.Points)
            .SingleOrDefaultAsync(cancellationToken);
    }
}