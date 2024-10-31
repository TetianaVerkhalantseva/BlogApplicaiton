namespace BlogApplicaiton.Services.AuthenticationServices.Models;

public sealed record SignInModel
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}