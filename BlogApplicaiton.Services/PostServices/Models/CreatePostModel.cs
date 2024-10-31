namespace BlogApplicaiton.Services.PostServices.Models;

public record CreatePostModel
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string BlogId { get; set; } = string.Empty;
}