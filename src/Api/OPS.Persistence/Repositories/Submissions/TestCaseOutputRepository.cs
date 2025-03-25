using OPS.Domain.Contracts.Repository.Submissions;
using OPS.Domain.Entities.Submit;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Submissions;

internal class TestCaseOutputRepository(AppDbContext dbContext) : Repository<TestCaseOutput>(dbContext), ITestCaseOutputRepository
{
    
}