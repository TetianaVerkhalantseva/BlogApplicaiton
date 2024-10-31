using Microsoft.AspNetCore.Mvc;

namespace BlogApplicaiton.Web.Views.Shared.Components.HeaderComponent;

public class HeaderComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync() => View();
}