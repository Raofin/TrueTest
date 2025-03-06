using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;

namespace OPS.Domain.Contracts;

public interface ITestCaseRepository : IBaseRepository<TestCase>
{
    Task<List<TestCase>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken);
}