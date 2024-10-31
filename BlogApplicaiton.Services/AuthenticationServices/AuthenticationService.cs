using System.Security.Claims;
using BlogApplicaiton.Database.Entities;
using BlogApplicaiton.EntityFramework.Repository;
using BlogApplicaiton.Services.AuthenticationServices.Models;
using BlogApplicaiton.Services.ResponseService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace BlogApplicaiton.Services.AuthenticationServices;

public class AuthenticationService : IAuthenticationService
{
    private const string NotFoundError = "Invalid credentials";
    private const string AlreadyExistsError = "User already exists";
    private const string CreateError = "Error while create new user";
    private const string AuthenticationFailedError = "Authentication failed";
    
    private readonly IGenericRepository<UserEntity> _userRepository;
    private readonly HttpContext _httpContext;

    public AuthenticationService(IGenericRepository<UserEntity> userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContext = httpContextAccessor.HttpContext;
    }
    
    public async Task<Response> SignIn(SignInModel model)
    {
        UserEntity? entity = await _userRepository.GetBy(user => user.Login == model.Login &&
                                                                 user.Password == model.Password);

        if (entity == null)
        {
            return Response.Error(NotFoundError);
        }

        return await SignInProcess(entity);
    }

    public async Task<Response> SignUp(SignUpModel model)
    {
        UserEntity? entity = await _userRepository.GetBy(user => user.Login == model.Login);

        if (entity != null)
        {
            return Response.Error(AlreadyExistsError);
        }

        entity = new UserEntity()
        {
            Id = Guid.NewGuid(),
            Login = model.Login,
            Password = model.Password,
        };

        try
        {
            await _userRepository.Create(entity);
        }
        catch
        {
            return Response.Error(CreateError);
        }

        return await SignInProcess(entity);
    }

    public Task SignOut() => _httpContext.SignOutAsync();

    private async Task<Response> SignInProcess(UserEntity user)
    {
        try
        {
            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, GetPrincipal(user));
        }
        catch
        {
            return Response.Error(AuthenticationFailedError);
        }

        return Response.OK();
    }

    private ClaimsPrincipal GetPrincipal(UserEntity user) =>
        new ClaimsPrincipal(new ClaimsIdentity(GetClaims(user), CookieAuthenticationDefaults.AuthenticationScheme));

    private IEnumerable<Claim> GetClaims(UserEntity user) =>
    [
        new Claim(TokenTypes.Iat, DateTime.Now.Ticks.ToString()),
        new Claim(TokenTypes.Sub, user.Id.ToString()),
        new Claim(TokenTypes.Name, user.Login),
    ];
}