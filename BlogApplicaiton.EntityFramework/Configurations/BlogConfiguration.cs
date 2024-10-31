using BlogApplicaiton.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApplicaiton.EntityFramework.Configurations;

public class BlogConfiguration : IEntityTypeConfiguration<BlogEntity>
{
    public void Configure(EntityTypeBuilder<BlogEntity> builder)
    {
        builder.ToTable("Blogs").HasKey(blog => blog.Id);

        builder.HasOne<UserEntity>(blog => blog.User)
            .WithMany(user => user.Blogs)
            .HasForeignKey(blog => blog.UserId);
    }
}