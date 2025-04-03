using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts.Repository.Exams;

public interface IExamCandidatesRepository : IBaseRepository<ExamCandidate>
{
    Task<bool> IsValidCandidate(Guid accountId, Guid examId, CancellationToken cancellationToken);
    Task<List<ExamCandidate>> GetExamParticipantsAsync(Guid examId, CancellationToken cancellationToken);
    Task<List<ExamCandidate>> GetExamCandidateByAccountAsync(Guid id, CancellationToken cancellationToken);
}