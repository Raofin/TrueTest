using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.Core;

namespace OPS.Domain.Entities.User;

public class Profile : SoftDeletableEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BioMarkdown { get; set; }
    public string? InstituteName { get; set; }
    public string? PhoneNumber { get; set; }

    public Guid AccountId { get; set; }
    public Guid? ImageFileId { get; set; }
    public Account Account { get; set; } = null!;
    public CloudFile? ImageFile { get; set; }
    public ICollection<ProfileSocial> ProfileSocials { get; set; } = [];
}