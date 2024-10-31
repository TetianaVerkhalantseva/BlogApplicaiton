using BlogApplicaiton.Services.PostServices;
using BlogApplicaiton.Services.PostServices.Models;
using BlogApplicaiton.Services.ResponseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplicaiton.Web.Controllers;

[Authorize]
public class PostController(IPostService postService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostModel model)
    {
        Response<Guid> response = await postService.Create(model);
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
    public async Task<IActionResult> Update([FromBody] UpdatePostModel model)
    {
        Response response = await postService.Update(model);
        if (response.IsError)
        {
            return BadRequest(new
            {
                errrorMessage = response.ErrorMessage,
            });
        }

        return Ok(new { success = true });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        Response response = await postService.Delete(id);
        if (response.IsError)
        {
            return BadRequest(new { message = response.ErrorMessage });
        }
        return Ok(new { success = true });
    }
 }