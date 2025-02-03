using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Entities.User;

public class Profile
{
    public long ProfileId { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string? BioMarkdown { get; set; }
    public string? InstituteName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsDeleted { get; set; }

    public long AccountId { get; set; }
    public Account Account { get; set; } = null!;
}