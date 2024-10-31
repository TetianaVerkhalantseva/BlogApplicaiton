using BlogApplicaiton.Database.Entities;
using BlogApplicaiton.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BlogApplicaiton.EntityFramework;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<BlogEntity> Blogs { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.Migrate();
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BlogConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}