using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Repositories;

internal class TestCaseOutputRepository(AppDbContext dbContext) : Repository<TestCaseOutput>(dbContext), ITestCaseOutputRepository
{
    
}