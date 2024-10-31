namespace BlogApplicaiton.Database.Entities;

public class BlogEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }

    public List<PostEntity> Posts { get; set; } = new List<PostEntity>();
}