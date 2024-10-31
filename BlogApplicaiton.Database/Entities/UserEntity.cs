namespace BlogApplicaiton.Database.Entities;

public sealed class UserEntity
{
    public Guid Id { get; set; }
    public string Login { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    
    public List<BlogEntity> Blogs { get; set; } = new List<BlogEntity>();
    public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
}