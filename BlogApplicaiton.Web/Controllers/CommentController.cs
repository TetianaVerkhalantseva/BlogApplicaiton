using BlogApplicaiton.Services.CommentServices;
using BlogApplicaiton.Services.CommentServices.Models;
using BlogApplicaiton.Services.ResponseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplicaiton.Web.Controllers;

[Authorize]
public class CommentController(ICommentService commentService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentModel model)
    {
        Response<Guid> response = await commentService.Create(model);
        if (response.IsError)
        {
            return BadRequest(new
            {
                errorMessage = response.ErrorMessage,
            });
        }

        return Ok(new
        {
            success = true,
            id = response.Result
        });
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateCommentModel model)
    {
        Response response = await commentService.Update(model);
        if (response.IsError)
        {
            return BadRequest(new
            {
                errorMessage = response.ErrorMessage,
            });
        }

        return Ok(new { success = true });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        Response response = await commentService.Delete(id);
        if (response.IsError)
        {
            return BadRequest(new
            {
                errorMessage = response.ErrorMessage,
            });
        }

        return Ok(new { success = true });
    }
}