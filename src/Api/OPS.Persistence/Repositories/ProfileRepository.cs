using OPS.Domain.Contracts;
using OPS.Domain.Entities.User;

namespace OPS.Persistence.Repositories;

internal class ProfileRepository(AppDbContext dbContext) : Repository<Profile>(dbContext), IProfileRepository
{
    private readonly AppDbContext _dbContext = dbContext;
}