namespace OPS.Service.Dtos;

public class ExamDto
{
    public long ExamId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime OpensAt { get; set; }
    public DateTime ClosesAt { get; set; }
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}
