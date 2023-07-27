using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Group7WebApp.Data;

public class AuthDbContext : IdentityDbContext<WebAppUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {

    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
    public object PostCategory { get; internal set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Posts)
            .UsingEntity(j => j.ToTable("PostCategory"));
        base.OnModelCreating(modelBuilder);

    }
    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);
    //    // Customize the ASP.NET Identity model and override the defaults if needed.
    //    // For example, you can rename the ASP.NET Identity table names and more.
    //    // Add your customizations after calling base.OnModelCreating(builder);
    //}

}
