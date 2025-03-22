using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts.Repository.Exams;

public interface IExamCandidatesRepository : IBaseRepository<ExamCandidate>
{
    Task<List<ExamCandidate>> GetExamCandidateAsync(CancellationToken cancellationToken);
    Task<List<ExamCandidate>> GetExamCandidateByAccountAsync(Guid id, CancellationToken cancellationToken);
}