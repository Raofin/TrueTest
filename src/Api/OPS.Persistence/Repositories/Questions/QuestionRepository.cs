using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Questions;
using OPS.Persistence.Repositories.Common;
using OPS.Domain.Entities.Exam;
using QuestionType = OPS.Domain.Enums.QuestionType;

namespace OPS.Persistence.Repositories.Questions;

internal class QuestionRepository(AppDbContext dbContext) : Repository<Question>(dbContext), IQuestionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Question?> GetWithTestCases(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Include(q => q.TestCases)
            .Where(q => q.Id == questionId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Question>> GetProblemSolvingByExamIdAsync(Guid examId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .AsNoTracking()
            .Include(q => q.TestCases)
            .Where(q => q.ExaminationId == examId && q.QuestionTypeId == (int)QuestionType.ProblemSolving)
            .OrderBy(exam => exam.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Question?> GetWithMcqOption(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Include(q => q.McqOption)
            .Where(q => q.Id == questionId)
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

    public async Task<Question?> GetWrittenByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Where(q => q.Id == id && q.QuestionTypeId == (int)QuestionType.Written)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Question>> GetWrittenByExamIdAsync(Guid examId, CancellationToken cancellationToken)
    {
        return await _dbContext.Questions
            .Where(q => q.ExaminationId == examId && q.QuestionTypeId == (int)QuestionType.Written)
            .ToListAsync(cancellationToken);
    }
}