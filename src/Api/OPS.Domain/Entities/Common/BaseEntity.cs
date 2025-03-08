namespace OPS.Domain.Entities.Common;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}