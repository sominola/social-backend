using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Social.Data.Entities;

namespace Social.Data.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.FirstName).IsRequired().HasColumnName("firstname").HasMaxLength(20);
        builder.Property(e => e.LastName).IsRequired().HasColumnName("lastname").HasMaxLength(20);
        builder.Property(e => e.Email).IsRequired().HasColumnName("email").HasMaxLength(40);
        builder.Property(e => e.HashedPassword).HasColumnName("hashed_password").HasMaxLength(100);
        builder.Property(e => e.RegisteredDate).HasColumnName("register_date");
    }
}