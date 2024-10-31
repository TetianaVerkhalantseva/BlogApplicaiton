using BlogApplicaiton.Database.Entities;
using BlogApplicaiton.EntityFramework.Repository;
using BlogApplicaiton.Services.CommentServices.Models;
using BlogApplicaiton.Services.ResponseService;

namespace BlogApplicaiton.Services.CommentServices;

public class CommentService(IGenericRepository<CommentEntity> repository) : ICommentService
{
    private const string NotFoundError = "Comment does not exist";
    private const string InvalidPostIdError = "Invalid post id";
    private const string InvalidUserIdError = "Invalid user id";
    private const string InvalidCommentIdError = "Invalid comment id";
    private const string CreateError = "Error while create comment";
    private const string UpdateError = "Error while update comment";
    private const string DeleteError = "Error while delete comment";

    public async Task<Response<Guid>> Create(CreateCommentModel model)
    {
        if (!Guid.TryParse(model.PostId, out Guid postId))
        {
            return Response<Guid>.Error(InvalidPostIdError);
        }
        
        if (!Guid.TryParse(model.UserId, out Guid userId))
        {
            return Response<Guid>.Error(InvalidUserIdError);
        }

        CommentEntity entity = new CommentEntity()
        {
            PostId = postId,
            UserId = userId,
            Content = model.Content,
            Id = Guid.NewGuid(),
            CreateDate = DateTime.Now.ToUniversalTime()
        };

        try
        {
            await repository.Create(entity);
        }
        catch
        {
            return Response<Guid>.Error(CreateError);
        }

        return Response<Guid>.OK(entity.Id);
    }

    public async Task<Response> Delete(Guid id)
    {
        CommentEntity? entity = await repository.GetById(id);
        if (entity == null)
        {
            return Response.Error(NotFoundError);
        }

        try
        {
            await repository.Delete(entity);
        }
        catch
        {
            return Response.Error(DeleteError);
        }

        return Response.OK();
    }

    public async Task<Response> Update(UpdateCommentModel model)
    {
        if (!Guid.TryParse(model.Id, out Guid commentId))
        {
            return Response.Error(InvalidCommentIdError);
        }
        
        CommentEntity? entity = await repository.GetById(commentId);
        if (entity == null)
        {
            return Response.Error(NotFoundError);
        }
        
        entity.Content = model.Content;
        entity.ModifyDate = DateTime.Now.ToUniversalTime();

        try
        {
            await repository.Update(entity);
        }
        catch
        {
            return Response.Error(UpdateError);
        }
        return Response.OK();
    }
}