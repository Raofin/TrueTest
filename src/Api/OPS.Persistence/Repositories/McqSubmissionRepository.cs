using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Repositories;

internal class McqSubmissionRepository(AppDbContext dbContext) : Repository<McqSubmission>(dbContext), IMcqSubmissionRepository
{
    private readonly AppDbContext _dbContext = dbContext;
}