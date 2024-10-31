using Microsoft.AspNetCore.Mvc;
using Moq;
using BlogApplicaiton.Services.CommentServices;
using BlogApplicaiton.Services.CommentServices.Models;
using BlogApplicaiton.Services.ResponseService;
using BlogApplicaiton.Web.Controllers;
using Xunit;

namespace BlogApplicaiton.Tests.Comment;

public class CommentControllerTests
{
    private readonly Mock<ICommentService> _mockCommentService;
    private readonly CommentController _controller;

    public CommentControllerTests()
    {
        _mockCommentService = new Mock<ICommentService>();
        _controller = new CommentController(_mockCommentService.Object);
    }
    
    [Fact]
    public async Task Create_Post_ValidModel_ReturnsOk()
    {
        var commentModel = new CreateCommentModel
        {
            Content = "Test Comment",
            PostId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        _mockCommentService.Setup(service => service.Create(commentModel))
            .ReturnsAsync(Response<Guid>.OK(Guid.NewGuid()));
        
        var result = await _controller.Create(commentModel);
        
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.NotNull(okResult.Value);
        _mockCommentService.Verify(service => service.Create(commentModel), Times.Once);
    }

    [Fact]
    public async Task Update_Post_ValidModel_ReturnsOk()
    {
        var updateModel = new UpdateCommentModel
        {
            Id = Guid.NewGuid().ToString(),
            Content = "Updated Comment"
        };
        _mockCommentService.Setup(service => service.Update(updateModel))
            .ReturnsAsync(Response.OK());
        
        var result = await _controller.Update(updateModel);
        
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.NotNull(okResult.Value);
        _mockCommentService.Verify(service => service.Update(updateModel), Times.Once);
    }

    [Fact]
    public async Task Delete_Post_ValidId_ReturnsOk()
    {
        var commentId = Guid.NewGuid();
        _mockCommentService.Setup(service => service.Delete(commentId))
            .ReturnsAsync(Response.OK());
        
        var result = await _controller.Delete(commentId);
        
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.NotNull(okResult.Value);
        _mockCommentService.Verify(service => service.Delete(commentId), Times.Once);
    }

    [Fact]
    public async Task Delete_Post_InvalidId_ReturnsBadRequest()
    {
        var commentId = Guid.NewGuid();
        _mockCommentService.Setup(service => service.Delete(commentId))
            .ReturnsAsync(Response.Error("Comment not found"));
        
        var result = await _controller.Delete(commentId);
        
        var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
        Xunit.Assert.NotNull(badRequestResult.Value);
    }
}
