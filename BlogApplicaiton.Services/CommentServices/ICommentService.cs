using BlogApplicaiton.Services.CommentServices.Models;
using BlogApplicaiton.Services.ResponseService;

namespace BlogApplicaiton.Services.CommentServices;

public interface ICommentService
{
    Task<Response<Guid>> Create(CreateCommentModel model);
    Task<Response> Delete(Guid id);
    Task<Response> Update(UpdateCommentModel model);
}