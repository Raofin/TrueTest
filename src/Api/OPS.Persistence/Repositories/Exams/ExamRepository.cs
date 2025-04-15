using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Exams;
using OPS.Domain.Entities.Exam;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Exams;

internal class ExamRepository(AppDbContext dbContext) : Repository<Examination>(dbContext), IExamRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Examination>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Examinations
            .AsNoTracking()
            .Where(e => e.ExamCandidates.Any(ec => ec.AccountId == accountId))
            .OrderBy(e => e.OpensAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Examination?> GetWithQuestionsAsync(Guid examId, CancellationToken cancellationToken)
    {
        var exam = await _dbContext.Examinations
            .AsNoTracking()
            .Where(e => e.Id == examId)
            .Include(e => e.Questions).ThenInclude(q => q.TestCases)
            .Include(e => e.Questions).ThenInclude(q => q.McqOption)
            .SingleOrDefaultAsync(cancellationToken);

        if (exam is not null)
        {
            exam.Questions = exam.Questions.OrderBy(q => q.CreatedAt).ToList();
        }

        return exam;
    }

    public async Task<ExamCandidate?> GetCandidateAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.ExamCandidates
            .Where(ec => ec.AccountId == accountId && ec.ExaminationId == examId)
            .Include(ec => ec.Account)
            .Include(ec => ec.Examination)
            .OrderBy(ec => ec.CreatedAt)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Examination?> GetWithQuesAndSubmissionsAsync(Guid examId, Guid accountId,
        CancellationToken cancellationToken)
    {
        var exam = await _dbContext.Examinations
            .AsNoTracking()
            .Where(e => e.Id == examId)
            .Include(e => e.ExamCandidates.Where(ec => ec.AccountId == accountId)).ThenInclude(ec => ec.Account)
            .Include(e => e.Questions).ThenInclude(q => q.ProblemSubmissions.Where(ps => ps.AccountId == accountId))
            .ThenInclude(tc => tc.TestCaseOutputs)
            .Include(e => e.Questions).ThenInclude(q => q.TestCases)
            .Include(e => e.Questions).ThenInclude(q => q.McqSubmissions.Where(s => s.AccountId == accountId))
            .Include(e => e.Questions).ThenInclude(q => q.McqOption)
            .Include(e => e.Questions).ThenInclude(q => q.WrittenSubmissions.Where(s => s.AccountId == accountId))
            .SingleOrDefaultAsync(cancellationToken);

        if (exam is not null)
        {
            exam.Questions = exam.Questions.OrderBy(q => q.CreatedAt).ToList();
        }

        return exam;
    }

    public async Task<Examination?> GetResultsAsync(Guid examId, CancellationToken cancellationToken)
    {
        var exam = await _dbContext.Examinations
            .AsNoTracking()
            .Where(e => e.Id == examId)
            .Include(e => e.ExamCandidates.Where(ec => ec.AccountId != null))
            .ThenInclude(ec => ec.Account)
            .SingleOrDefaultAsync(cancellationToken);

        if (exam is not null)
        {
            exam.ExamCandidates = exam.ExamCandidates.OrderBy(ec => ec.SubmittedAt).ToList();
        }

        return exam;
    }

    public async Task<bool> IsPublished(Guid questionId, CancellationToken cancellationToken)
    {
        return await _dbContext.TestCases
            .AsNoTracking()
            .Where(q => q.QuestionId == questionId)
            .Select(q => q.Question.Examination.IsPublished)
            .FirstAsync(cancellationToken);
    }
}