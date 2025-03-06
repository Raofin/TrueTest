using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.User;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.User;

public class AdminInvitesConfiguration : IEntityTypeConfiguration<AdminInvite>
{
    public void Configure(EntityTypeBuilder<AdminInvite> entity)
    {
        entity.ToTable("AdminInvites", "User");
        entity.HasKey(e => e.Id);
        
        new BaseEntityConfig<AdminInvite>().Configure(entity);
    }
}