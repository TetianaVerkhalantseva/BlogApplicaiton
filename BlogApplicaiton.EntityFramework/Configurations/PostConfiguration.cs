using BlogApplicaiton.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApplicaiton.EntityFramework.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.ToTable("Posts").HasKey(post => post.Id);
        
        builder.HasOne<BlogEntity>(post => post.Blog)
            .WithMany(blog => blog.Posts)
            .HasForeignKey(post => post.BlogId);
    }
}