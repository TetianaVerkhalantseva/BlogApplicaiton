using Microsoft.AspNetCore.Mvc;
using Moq;
using BlogApplicaiton.Services.AuthenticationServices;
using BlogApplicaiton.Services.AuthenticationServices.Models;
using BlogApplicaiton.Services.ResponseService;
using BlogApplicaiton.Web.Controllers;
using Xunit;

namespace BlogApplicaiton.Tests.Authentication;

public class AuthenticationControllerTests
{
    private readonly Mock<IAuthenticationService> _mockAuthService;
    private readonly AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _mockAuthService = new Mock<IAuthenticationService>();
        _controller = new AuthenticationController(_mockAuthService.Object);
    }
    
    [Fact]
    public async Task SignIn_Post_ValidModel_ReturnsOk()
    {
        var signInModel = new SignInModel
        {
            Login = "testuser",
            Password = "password"
        };
        _mockAuthService.Setup(service => service.SignIn(signInModel))
            .ReturnsAsync(Response.OK());
        
        var result = await _controller.SignIn(signInModel);
        
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.NotNull(okResult.Value);
        _mockAuthService.Verify(service => service.SignIn(signInModel), Times.Once);
    }

    [Fact]
    public async Task SignIn_Post_InvalidCredentials_ReturnsBadRequest()
    {
        var signInModel = new SignInModel
        {
            Login = "wronguser",
            Password = "wrongpassword"
        };
        _mockAuthService.Setup(service => service.SignIn(signInModel))
            .ReturnsAsync(Response.Error("Invalid credentials"));
        
        var result = await _controller.SignIn(signInModel);
        
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.NotNull(badRequestResult.Value);
        _mockAuthService.Verify(service => service.SignIn(signInModel), Times.Once);
    }

    [Fact]
    public async Task SignUp_Post_ValidModel_ReturnsOk()
    {
        var signUpModel = new SignUpModel
        {
            Login = "newuser",
            Password = "newpassword"
        };
        _mockAuthService.Setup(service => service.SignUp(signUpModel))
            .ReturnsAsync(Response.OK());
        
        var result = await _controller.SignUp(signUpModel);
        
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.NotNull(okResult.Value);
        _mockAuthService.Verify(service => service.SignUp(signUpModel), Times.Once);
    }

    [Fact]
    public async Task SignUp_Post_UserAlreadyExists_ReturnsBadRequest()
    {
        var signUpModel = new SignUpModel
        {
            Login = "existinguser",
            Password = "password"
        };
        _mockAuthService.Setup(service => service.SignUp(signUpModel))
            .ReturnsAsync(Response.Error("User already exists"));
        
        var result = await _controller.SignUp(signUpModel);
        
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.NotNull(badRequestResult.Value);
        _mockAuthService.Verify(service => service.SignUp(signUpModel), Times.Once);
    }

    [Fact]
    public async Task SignOut_Get_RedirectsToHome()
    {
        var result = await _controller.SignOut();
        
        var redirectResult = Xunit.Assert.IsType<RedirectToActionResult>(result);
        Xunit.Assert.Equal("Index", redirectResult.ActionName);
        Xunit.Assert.Equal("Home", redirectResult.ControllerName);
        _mockAuthService.Verify(service => service.SignOut(), Times.Once);
    }
}
