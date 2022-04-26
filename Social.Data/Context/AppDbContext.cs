using Microsoft.EntityFrameworkCore;
using Social.Data.Configurations;
using Social.Data.Entities;

namespace Social.Data.Context;

public sealed class AppDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}