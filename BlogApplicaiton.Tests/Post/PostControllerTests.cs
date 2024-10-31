using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BlogApplicaiton.Services.PostServices;
using BlogApplicaiton.Services.PostServices.Models;
using BlogApplicaiton.Services.ResponseService;
using BlogApplicaiton.Web.Controllers;
using Xunit;

namespace BlogApplicaiton.Tests.Post;

public class PostControllerTests
{
    private readonly Mock<IPostService> _mockPostService;
    private readonly PostController _controller;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly ClaimsPrincipal _user;

    public PostControllerTests()
    {
        _mockPostService = new Mock<IPostService>();
        _controller = new PostController(_mockPostService.Object);
        _mockHttpContext = new Mock<HttpContext>();
        _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
        }, "mock"));

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = _mockHttpContext.Object
        };
        _mockHttpContext.SetupGet(x => x.User).Returns(_user);
    }

    [Fact]
    public async Task Create_Post_InvalidModelState_ReturnsBadRequest()
    {
        var mockPostService = new Mock<IPostService>();
        
        mockPostService.Setup(service => service.Create(It.IsAny<CreatePostModel>()))
            .ReturnsAsync(Response<Guid>.Error("Model state is invalid"));

        var controller = new PostController(mockPostService.Object);
        
        controller.ModelState.AddModelError("Title", "Required");
        var postModel = new CreatePostModel();
        
        var result = await controller.Create(postModel);
        
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Create_Post_ValidModel_ReturnsOk()
    {
        var postModel = new CreatePostModel
        {
            Title = "Test Post",
            Content = "Test Content",
            BlogId = Guid.NewGuid().ToString()
        };
        _mockPostService.Setup(service => service.Create(postModel))
            .ReturnsAsync(Response<Guid>.OK(Guid.NewGuid()));
        
        var result = await _controller.Create(postModel);
        
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.NotNull(okResult.Value);
        _mockPostService.Verify(service => service.Create(postModel), Times.Once);
    }

    [Fact]
    public async Task Update_Post_ValidModel_ReturnsOk()
    {
        var updateModel = new UpdatePostModel
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Updated Post",
            Content = "Updated Content"
        };
        _mockPostService.Setup(service => service.Update(updateModel))
            .ReturnsAsync(Response.OK());
        
        var result = await _controller.Update(updateModel);
        
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.NotNull(okResult.Value);
        _mockPostService.Verify(service => service.Update(updateModel), Times.Once);
    }

    [Fact]
    public async Task Update_Post_InvalidModelState_ReturnsBadRequest()
    {
        var mockPostService = new Mock<IPostService>();
        
        mockPostService.Setup(service => service.Update(It.IsAny<UpdatePostModel>()))
            .ReturnsAsync(Response.Error("Model state is invalid"));

        var controller = new PostController(mockPostService.Object);
        
        controller.ModelState.AddModelError("Title", "Required");
        var updateModel = new UpdatePostModel();
        
        var result = await controller.Update(updateModel);
        
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Delete_Post_ValidId_ReturnsOk()
    {
        var postId = Guid.NewGuid();
        _mockPostService.Setup(service => service.Delete(postId))
            .ReturnsAsync(Response.OK());
        
        var result = await _controller.Delete(postId);
        
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.NotNull(okResult.Value);
        _mockPostService.Verify(service => service.Delete(postId), Times.Once);
    }

    [Fact]
    public async Task Delete_Post_InvalidId_ReturnsBadRequest()
    {
        var postId = Guid.NewGuid();
        _mockPostService.Setup(service => service.Delete(postId))
            .ReturnsAsync(Response.Error("Post not found"));
        
        var result = await _controller.Delete(postId);
        
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.NotNull(badRequestResult.Value);
    }
}
