namespace BlogApplicaiton.Database.Entities;

public class CommentEntity
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public DateTime ModifyDate { get; set; }
    
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
}