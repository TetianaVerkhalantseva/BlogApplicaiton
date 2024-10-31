namespace BlogApplicaiton.Services.CommentServices.Models;

public record CreateCommentModel
{
    public string Content { get; set; } = string.Empty;
    public string PostId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}