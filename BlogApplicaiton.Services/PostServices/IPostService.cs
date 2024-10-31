using BlogApplicaiton.Services.PostServices.Models;
using BlogApplicaiton.Services.ResponseService;

namespace BlogApplicaiton.Services.PostServices;

public interface IPostService
{
    Task<Response<Guid>> Create(CreatePostModel model);
    Task<Response> Delete(Guid id);
    Task<Response> Update(UpdatePostModel model);
}