using BlogApplicaiton.Database.Entities;
using BlogApplicaiton.Services.BlogServices;
using BlogApplicaiton.Services.BlogServices.Models;
using BlogApplicaiton.Services.ResponseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplicaiton.Web.Controllers;

[Authorize]
public class BlogController(IBlogService blogService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> List() => View(await blogService.GetAll());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBlogModel model)
    {
        Response<Guid> response = await blogService.Create(model);
        if (response.IsError)
        {
            return BadRequest(new
            {
                errorMessage = response.ErrorMessage,
            });
        }

        return Ok(new { success = true, id = response.Result });
    }

    [HttpGet]
    public async Task<IActionResult> Details([FromRoute] Guid id)
    {
        Response<BlogEntity> getBlogResult = await blogService.GetById(id);
        if (getBlogResult.IsError)
        {
            return RedirectToAction("NotFound", "Home");
        }
        return View(getBlogResult.Result);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePublicStatus([FromBody] ChangePublicModel model)
    {
        Response response = await blogService.ChangePublic(model);
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
        });
    }
    
}