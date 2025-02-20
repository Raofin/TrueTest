using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts;

public interface IExamCandidatesRepository : IBaseRepository<ExamCandidate>
{
    Task<List<ExamCandidate>> GetExamCandidateAsync(CancellationToken cancellationToken);
    Task<List<ExamCandidate>> GetExamCandidateByAccountAsync(Guid id, CancellationToken cancellationToken);
}