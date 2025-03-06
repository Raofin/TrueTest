using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OPS.Domain.Entities.User;
using OPS.Persistence.Configurations.Common;

namespace OPS.Persistence.Configurations.User;

public class AdminInvitesConfiguration : IEntityTypeConfiguration<AdminInvites>
{
    public void Configure(EntityTypeBuilder<AdminInvites> entity)
    {
        new BaseEntityConfig<AdminInvites>().Configure(entity);
    }
}