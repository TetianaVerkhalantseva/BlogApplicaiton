using BlogApplicaiton.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplicaiton.Web.Controllers;

public class HomeController(IUserContext userContext) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        if (userContext.IsAuthenticated == true)
        {
            return RedirectToAction("List", "Blog");
        }
        
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> NotFound(string message) => View("NotFound", message);
}