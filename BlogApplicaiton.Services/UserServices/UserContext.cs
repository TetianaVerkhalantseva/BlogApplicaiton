using BlogApplicaiton.Services.AuthenticationServices;
using Microsoft.AspNetCore.Http;

namespace BlogApplicaiton.Services.UserServices;

public class UserContext : IUserContext
{
    private readonly HttpContext _httpContext;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public Guid? Id
    {
        get
        {
            string? value = _httpContext.User.Claims?.FirstOrDefault(claim => claim.Type == TokenTypes.Sub)?.Value;
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (!Guid.TryParse(value, out Guid id))
            {
                return null;
            }

            return id;
        }
    }

    public string? Login
    {
        get
        {
            return _httpContext.User.Claims?.FirstOrDefault(claim => claim.Type == TokenTypes.Name)?.Value;
        }
    }

    public bool? IsAuthenticated => _httpContext.User.Identity?.IsAuthenticated;
}