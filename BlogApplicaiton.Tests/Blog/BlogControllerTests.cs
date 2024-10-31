using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BlogApplicaiton.Database.Entities;
using BlogApplicaiton.Services.BlogServices;
using BlogApplicaiton.Services.BlogServices.Models;
using BlogApplicaiton.Services.ResponseService;
using BlogApplicaiton.Web.Controllers;
using Xunit;

namespace BlogApplicaiton.Tests.Blog;

public class BlogControllerTests
{
    private readonly Mock<IBlogService> _mockBlogService;
    private readonly BlogController _controller;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly ClaimsPrincipal _user;

    public BlogControllerTests()
    {
        _mockBlogService = new Mock<IBlogService>();
        _controller = new BlogController(_mockBlogService.Object);
        
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
        var mockBlogService = new Mock<IBlogService>();
        
        mockBlogService.Setup(service => service.Create(It.IsAny<CreateBlogModel>()))
            .ReturnsAsync(Response<Guid>.Error("Model state is invalid"));

        var controller = new BlogController(mockBlogService.Object);
        
        controller.ModelState.AddModelError("Title", "Required");
        var blogPost = new CreateBlogModel();
        
        var result = await controller.Create(blogPost);
        
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task Create_Post_ValidModel_CallsBlogServiceAndReturnsOk()
    {
        var blogPost = new CreateBlogModel
        {
            Title = "Test Blog",
            IsPublic = true
        };
        _mockBlogService.Setup(service => service.Create(blogPost))
            .ReturnsAsync(Response<Guid>.OK(Guid.NewGuid()));
        
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
        }));
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };
        
        var result = await _controller.Create(blogPost);
        
        _mockBlogService.Verify(service => service.Create(blogPost), Times.Once);
        Xunit.Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task List_ReturnsViewWithCorrectModel()
    {
        var mockBlogs = new List<BlogEntity>
        {
            new BlogEntity { Title = "Test Blog 1" },
            new BlogEntity { Title = "Test Blog 2"}
        };
        _mockBlogService.Setup(service => service.GetAll())
            .ReturnsAsync(mockBlogs);
        
        var result = await _controller.List();
        
        var viewResult = Xunit.Assert.IsType<ViewResult>(result);
        var model = Xunit.Assert.IsAssignableFrom<List<BlogEntity>>(viewResult.Model);
        Xunit.Assert.Equal(mockBlogs, model);
    }

    [Fact]
    public async Task Details_WithValidId_ReturnsViewWithCorrectModel()
    {
        var blogId = Guid.NewGuid();
        var mockBlog = new BlogEntity
        {
            Title = "Test Blog"
        };
        _mockBlogService.Setup(service => service.GetById(blogId))
            .ReturnsAsync(Response<BlogEntity>.OK(mockBlog));
        
        var result = await _controller.Details(blogId);
        
        var viewResult = Xunit.Assert.IsType<ViewResult>(result);
        var model = Xunit.Assert.IsAssignableFrom<BlogEntity>(viewResult.Model);
        Xunit.Assert.Equal(mockBlog, model);
    }

    [Fact]
    public async Task Details_WithInvalidId_ReturnsRedirectToNotFound()
    {
        var blogId = Guid.NewGuid();
        _mockBlogService.Setup(service => service.GetById(blogId))
            .ReturnsAsync(Response<BlogEntity>.Error("No blog with that id"));
        
        var result = await _controller.Details(blogId);
        
        var redirectResult = Xunit.Assert.IsType<RedirectToActionResult>(result);
        Xunit.Assert.Equal("NotFound", redirectResult.ActionName);
        Xunit.Assert.Equal("Home", redirectResult.ControllerName);
    }

    [Fact]
    public async Task ChangePublicStatus_InvalidModel_ReturnsBadRequest()
    {
        var changePublicModel = new ChangePublicModel
        {
            Id = Guid.NewGuid().ToString(),
            IsPublic = false
        };
        _mockBlogService.Setup(service => service.ChangePublic(changePublicModel))
            .ReturnsAsync(Response.Error("Error changing public status"));
        
        var result = await _controller.ChangePublicStatus(changePublicModel);
        
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.NotNull(badRequestResult.Value);
    }

    [Fact]
    public async Task ChangePublicStatus_ValidModel_ReturnsOk()
    {
        var changePublicModel = new ChangePublicModel
        {
            Id = Guid.NewGuid().ToString(),
            IsPublic = true
        };
        _mockBlogService.Setup(service => service.ChangePublic(changePublicModel))
            .ReturnsAsync(Response.OK());
        
        var result = await _controller.ChangePublicStatus(changePublicModel);
        
        _mockBlogService.Verify(service => service.ChangePublic(changePublicModel), Times.Once);
        Xunit.Assert.IsType<OkObjectResult>(result);
    }
}
