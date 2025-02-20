namespace OPS.Domain.Entities.Common;

public interface IBaseEntity
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}