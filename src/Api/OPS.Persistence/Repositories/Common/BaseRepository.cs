﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.Common;
using OPS.Domain.Interfaces.Common;

namespace OPS.Persistence.Repositories.Common;

public class Repository<TEntity>(AppDbContext context) : IBaseRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _entities = context.Set<TEntity>();

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _entities.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _entities.CountAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<TResult>> SelectAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        return await _entities.Where(predicate).Select(selector).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _entities.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _entities.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _entities.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _entities.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<PaginatedList<TEntity>> GetPaginatedListAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _entities.AsQueryable();
        return await PaginatedList<TEntity>.CreateAsync(query, pageIndex, pageSize, cancellationToken);
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
}