namespace BlogApplicaiton.Database.Entities;

public class PostEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    
    public Guid BlogId { get; set; }
    public BlogEntity Blog { get; set; }
    
    public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
}