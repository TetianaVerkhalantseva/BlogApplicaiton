using BlogApplicaiton.EntityFramework;
using BlogApplicaiton.EntityFramework.Repository;
using BlogApplicaiton.Services.AuthenticationServices;
using BlogApplicaiton.Services.BlogServices;
using BlogApplicaiton.Services.CommentServices;
using BlogApplicaiton.Services.PostServices;
using BlogApplicaiton.Services.UserServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.AddControllersWithViews();

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/authentication/SignIn";
        options.LogoutPath = "/authentication/SignIn";
        options.LoginPath = "/authentication/SignIn";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
    });

services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=BlogDb.db;")); // Prop/Dev env
services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

services.AddHttpContextAccessor();
services.AddScoped<IUserContext, UserContext>();
services.AddTransient<IAuthenticationService, AuthenticationService>();
services.AddTransient<IBlogService, BlogService>();
services.AddTransient<IPostService, PostService>();
services.AddTransient<ICommentService, CommentService>();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();