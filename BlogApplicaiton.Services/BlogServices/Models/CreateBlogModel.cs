namespace BlogApplicaiton.Services.BlogServices.Models;

public sealed record CreateBlogModel
{
    public string Title { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public string UserId { get; set; } = string.Empty;
}