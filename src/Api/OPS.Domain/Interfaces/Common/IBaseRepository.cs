using System.Linq.Expressions;
using OPS.Domain.Entities.Common;

namespace OPS.Domain.Interfaces.Common;

/// <summary>
/// Defines the contract for a generic repository providing asynchronous data access operations for entities.
/// </summary>
/// <typeparam name="TEntity">The type of entity that the repository manages.</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Asynchronously selects a collection of results from entities that satisfy a predicate, using a selector.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to select.</typeparam>
    /// <param name="predicate">An expression to filter the entities.</param>
    /// <param name="selector">An expression to select the desired properties or transform the entity into a result.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of the selected results.</returns>
    Task<IEnumerable<TResult>> SelectAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found, otherwise null.</returns>
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all entities.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all entities.</returns>
    Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously finds a collection of entities that satisfy a predicate.
    /// </summary>
    /// <param name="predicate">An expression to filter the entities.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities that satisfy the predicate.</returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the first entity that satisfies a predicate, or a default value if no such entity is found.
    /// </summary>
    /// <param name="predicate">An expression to filter the entities.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that satisfies the predicate, or a default value if not found.</returns>
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a paginated list of entities.
    /// </summary>
    /// <param name="pageIndex">The zero-based index of the page to retrieve.</param>
    /// <param name="pageSize">The number of entities to include in each page.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a paginated list of entities.</returns>
    Task<PaginatedList<TEntity>> GetPaginatedListAsync(int pageIndex, int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously counts the number of entities that satisfy a predicate.
    /// </summary>
    /// <param name="predicate">An expression to filter the entities.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities that satisfy the predicate.</returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously checks if any entity satisfies a predicate.
    /// </summary>
    /// <param name="predicate">An expression to filter the entities.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if at least one entity satisfies the predicate, otherwise false.</returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    void Add(TEntity entity);

    /// <summary>
    /// Adds a range of new entities to the repository.
    /// </summary>
    /// <param name="entities">The collection of entities to add.</param>
    void AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Removes an existing entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    void Remove(TEntity entity);

    /// <summary>
    /// Removes a range of existing entities from the repository.
    /// </summary>
    /// <param name="entities">The collection of entities to remove.</param>
    void RemoveRange(IEnumerable<TEntity> entities);
}