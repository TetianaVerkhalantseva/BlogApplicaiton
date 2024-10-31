namespace BlogApplicaiton.Services.CommentServices.Models;

public record UpdateCommentModel
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}