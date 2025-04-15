namespace OPS.Domain.Entities.Common;

public abstract class SoftDeletableEntity : BaseEntity, ISoftDeletable
{
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}