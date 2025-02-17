using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Contracts;

public interface IWrittenSubmissionRepository : IRepository<WrittenSubmission>
{
    Task<List<WrittenSubmission>> GetAllWrittenSubmissionByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken);
}