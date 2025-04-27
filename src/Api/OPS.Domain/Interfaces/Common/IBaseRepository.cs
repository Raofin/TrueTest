using System.Linq.Expressions;
using OPS.Domain.Entities.Common;

namespace OPS.Domain.Interfaces.Common;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TResult>> SelectAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<PaginatedList<TEntity>> GetPaginatedListAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}