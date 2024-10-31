using BlogApplicaiton.Database.Entities;
using BlogApplicaiton.Services.BlogServices.Models;
using BlogApplicaiton.Services.ResponseService;

namespace BlogApplicaiton.Services.BlogServices;

public interface IBlogService
{
    Task<Response<Guid>> Create(CreateBlogModel model);
    Task<List<BlogEntity>> GetAll();
    Task<Response<BlogEntity>> GetById(Guid id);
    Task<Response> ChangePublic(ChangePublicModel model);
}