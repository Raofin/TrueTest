using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<List<Question>> GetAllQuestionByExamIdAsync(Guid id, CancellationToken cancellationToken);
}