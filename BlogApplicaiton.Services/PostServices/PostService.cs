using BlogApplicaiton.Database.Entities;
using BlogApplicaiton.EntityFramework.Repository;
using BlogApplicaiton.Services.PostServices.Models;
using BlogApplicaiton.Services.ResponseService;

namespace BlogApplicaiton.Services.PostServices;

public sealed class PostService(IGenericRepository<PostEntity> repository) : IPostService
{
    private const string AlreadyExistsPost = "Post with this title already exists in blog";
    private const string NotFoundError = "Post with this title does not exist";
    private const string InvalidBlogIdError = "Invalid blog id";
    private const string InvalidPostIdError = "Invalid post id";
    private const string CreateError = "Error while create post";
    private const string UpdateError = "Error while update post";
    private const string DeleteError = "Error while delete post";
    
    public async Task<Response<Guid>> Create(CreatePostModel model)
    {
        if (!Guid.TryParse(model.BlogId, out Guid blogId))
        {
            return Response<Guid>.Error(InvalidBlogIdError);
        }
        
        PostEntity? entity = await repository.GetBy(post => post.Title == model.Title &&
                                                            post.BlogId == blogId);

        if (entity != null)
        {
            return Response<Guid>.Error(AlreadyExistsPost); 
        }

        entity = new PostEntity()
        {
            Title = model.Title,
            BlogId = blogId,
            Content = model.Content,
            Id = Guid.NewGuid(),
            PublishDate = DateTime.Now.ToUniversalTime()
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
        PostEntity? entity = await repository.GetById(id);
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

    public async Task<Response> Update(UpdatePostModel model)
    {
        if (!Guid.TryParse(model.Id, out Guid postId))
        {
            return Response.Error(InvalidPostIdError);
        }
        
        PostEntity? entity = await repository.GetById(postId);
        if (entity == null)
        {
            return Response.Error(NotFoundError);
        }
        
        entity.Title = model.Title;
        entity.Content = model.Content;
        entity.ModifiedDate = DateTime.Now.ToUniversalTime();

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