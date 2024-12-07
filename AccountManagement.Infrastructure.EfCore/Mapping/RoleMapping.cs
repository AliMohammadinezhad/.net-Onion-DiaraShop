using AccountManagement.Domain.RoleAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountManagement.Infrastructure.EfCore.Mapping;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(128);

        builder.HasMany(x => x.Accounts).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
        builder.OwnsMany(x => x.Permissions, navigationBuilder =>
        {
            navigationBuilder.HasKey(x => x.Id);
            navigationBuilder.ToTable("Permissions");
            navigationBuilder.WithOwner(x => x.Role);
            navigationBuilder.Ignore(x => x.Name);
        });
    }
}