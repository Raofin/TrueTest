using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Exams;

public interface IExamCandidatesRepository : IBaseRepository<ExamCandidate>
{
    Task<bool> IsValidCandidate(Guid accountId, Guid examId, CancellationToken cancellationToken);
    Task<ExamCandidate?> GetAsync(Guid accountId, Guid examId, CancellationToken cancellationToken);
    Task<List<ExamCandidate>> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<List<string>> GetEmailsByExamAsync(Guid examId, List<string> emails, CancellationToken cancellationToken);
    Task<List<ExamCandidate>> GetExamParticipantsAsync(Guid examId, CancellationToken cancellationToken);
    Task<List<ExamCandidate>> GetExamCandidateByAccountAsync(Guid id, CancellationToken cancellationToken);
}