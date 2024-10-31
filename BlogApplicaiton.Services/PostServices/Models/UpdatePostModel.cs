namespace BlogApplicaiton.Services.PostServices.Models;

public record UpdatePostModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}