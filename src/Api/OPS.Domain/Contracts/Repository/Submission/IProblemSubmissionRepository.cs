using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Contracts;

public interface IProblemSubmissionRepository : IBaseRepository<ProblemSubmission>
{
    Task<ProblemSubmission?> GetWithOutputsAsync(Guid questionId, Guid accountId, CancellationToken cancellationToken);
    Task<ProblemSubmission?> GetWithOutputsAsync(Guid problemSubmissionId, CancellationToken cancellationToken);
    Task<List<Question>> GetAllProblemsWithSubmission(Guid examId, Guid accountId, CancellationToken cancellationToken);
}