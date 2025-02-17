using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts;

public interface IQuestionRepository : IRepository<Question>
{
   Task<List<Question>> GetAllQuestionByExamIdAsync(Guid id, CancellationToken cancellationToken);

}