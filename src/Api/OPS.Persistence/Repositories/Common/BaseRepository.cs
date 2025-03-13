using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using OPS.Domain.Contracts.Repository;

namespace OPS.Persistence.Repositories;

internal class Repository<TEntity>(AppDbContext context) : IBaseRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _entities.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _entities.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _entities.Where(e => EF.Property<Guid>(e, "Id").Equals(id)).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await _entities.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await _entities.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public void Add(TEntity entity)
    {
        _entities.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _entities.AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _entities.RemoveRange(entities);
    }
    
    public void Update(TEntity entity)
    {
        _entities.Update(entity);
    }
}