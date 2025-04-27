using OPS.Domain.Entities.Exam;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Questions;

public interface IQuestionRepository : IBaseRepository<Question>
{
    Task<Question?> GetWithTestCases(Guid questionId, CancellationToken cancellationToken);
    Task<List<Question>> GetProblemSolvingByExamIdAsync(Guid examId, CancellationToken cancellationToken);
    Task<Question?> GetWithMcqOption(Guid questionId, CancellationToken cancellationToken);
    Task<List<Question>> GetMcqByExamIdAsync(Guid examId, CancellationToken cancellationToken);
    Task<Question?> GetWrittenByIdAsync(Guid questionId, CancellationToken cancellationToken);
    Task<Question?> GetWithExamAsync(Guid questionId, CancellationToken cancellationToken);
    Task<List<Question>> GetWrittenByExamIdAsync(Guid examId, CancellationToken cancellationToken);
    Task<decimal> GetPointsAsync(Guid questionId, CancellationToken cancellationToken);
}