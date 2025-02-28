using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Submit;

namespace OPS.Persistence.Repositories;

internal class ProblemSubmissionRepository(AppDbContext dbContext) : Repository<ProblemSubmission>(dbContext), IProblemSubmissionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

}