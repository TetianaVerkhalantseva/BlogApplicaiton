using BlogApplicaiton.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApplicaiton.EntityFramework.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.ToTable("Comments").HasKey(comment => comment.Id);
        
        builder.HasOne<UserEntity>( comment => comment.User)
            .WithMany(user => user.Comments)
            .HasForeignKey(comment => comment.UserId);
        
        builder.HasOne<PostEntity>(comment => comment.Post)
            .WithMany(post => post.Comments)
            .HasForeignKey(comment => comment.PostId);
    }
}