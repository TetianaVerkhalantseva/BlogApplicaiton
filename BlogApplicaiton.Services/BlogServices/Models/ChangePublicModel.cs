namespace BlogApplicaiton.Services.BlogServices.Models;

public sealed record ChangePublicModel
{
    public string Id { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
}