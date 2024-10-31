using BlogApplicaiton.Services.AuthenticationServices.Models;
using BlogApplicaiton.Services.ResponseService;

namespace BlogApplicaiton.Services.AuthenticationServices;

public interface IAuthenticationService
{
    Task<Response> SignIn(SignInModel model);
    Task<Response> SignUp(SignUpModel model);
    Task SignOut();
}