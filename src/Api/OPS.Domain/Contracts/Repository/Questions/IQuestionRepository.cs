using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Exam;

namespace OPS.Domain.Contracts.Repository.Questions;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<Question?> GetWithTestCases(Guid questionId, CancellationToken cancellationToken);
    Task<List<Question>> GetProblemSolvingByExamIdAsync(Guid examId, CancellationToken cancellationToken);
    Task<Question?> GetWithMcqOption(Guid questionId, CancellationToken cancellationToken);
    Task<List<Question>> GetMcqByExamIdAsync(Guid examId, CancellationToken cancellationToken);
    Task<Question?> GetWrittenByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Question>> GetWrittenByExamIdAsync(Guid examId, CancellationToken cancellationToken);
}