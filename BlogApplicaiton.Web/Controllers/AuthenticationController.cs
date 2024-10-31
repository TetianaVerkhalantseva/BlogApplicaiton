using BlogApplicaiton.Services.AuthenticationServices;
using BlogApplicaiton.Services.AuthenticationServices.Models;
using BlogApplicaiton.Services.ResponseService;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplicaiton.Web.Controllers;

public class AuthenticationController(IAuthenticationService authenticationService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> SignIn() => View();

    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] SignInModel model)
    {
        Response signInResponse = await authenticationService.SignIn(model);
        if (signInResponse.IsError)
        {
            return BadRequest(new
            {
                errorMessage = signInResponse.ErrorMessage
            });
        }

        return Ok(new { success = true });
    }
    
    [HttpGet]
    public async Task<IActionResult> SignUp() => View();

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
    {
        Response signUpResponse = await authenticationService.SignUp(model);
        if (signUpResponse.IsError)
        {
            return BadRequest(new
            {
                errorMessage = signUpResponse.ErrorMessage
            });
        }

        return Ok(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> SignOut()
    {
        await authenticationService.SignOut();
        return RedirectToAction("Index", "Home");
    }
}