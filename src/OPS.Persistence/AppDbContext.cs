using Microsoft.EntityFrameworkCore;

namespace OPS.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
