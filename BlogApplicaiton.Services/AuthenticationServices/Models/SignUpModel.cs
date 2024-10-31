namespace BlogApplicaiton.Services.AuthenticationServices.Models;

public sealed record SignUpModel
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}