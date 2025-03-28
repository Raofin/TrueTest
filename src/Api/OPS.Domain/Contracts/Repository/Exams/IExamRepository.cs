using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts.Repository.Exams;

public interface IExamRepository : IBaseRepository<Examination>
{
    Task<List<Examination>> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task<Examination?> GetWithQuestionsAsync(Guid examId, CancellationToken cancellationToken);
    Task<ExamCandidate?> GetCandidateAsync(Guid examId, Guid accountId, CancellationToken cancellationToken);
    Task<Examination?> GetWithQuesAndSubmissionsAsync(Guid examId, Guid accountId, CancellationToken cancellationToken);
}