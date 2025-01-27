using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities;

namespace OPS.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserDetail> UserDetails { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        //modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}
