using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social.Data.Entities;

namespace Social.Data.Configurations;

public class RoleConfiguration: IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).IsRequired().HasColumnName("name").HasMaxLength(50);
        builder.HasMany(e => e.Users).WithMany(x => x.Roles).UsingEntity(j=>j.ToTable("UserRoles"));
    }
}