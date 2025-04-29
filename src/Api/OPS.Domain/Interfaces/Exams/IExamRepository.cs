using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Exams;

public interface IExamRepository : IBaseRepository<Examination>
{
    Task<List<Examination>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task<Examination?> GetWithQuestionsAsync(Guid examId, CancellationToken cancellationToken);
    Task<ExamCandidate?> GetCandidateAsync(Guid examId, Guid accountId, CancellationToken cancellationToken);
    Task<Examination?> GetWithQuesAndSubmissionsAsync(Guid examId, Guid accountId, CancellationToken cancellationToken);
    Task<Examination?> GetResultsAsync(Guid examId, CancellationToken cancellationToken);
    Task<bool> IsPublished(Guid questionId, CancellationToken cancellationToken);
}