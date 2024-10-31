using BlogApplicaiton.Database.Entities;
using BlogApplicaiton.EntityFramework.Repository;
using BlogApplicaiton.Services.BlogServices.Models;
using BlogApplicaiton.Services.ResponseService;
using Microsoft.EntityFrameworkCore;

namespace BlogApplicaiton.Services.BlogServices;

public sealed class BlogService(IGenericRepository<BlogEntity> repository) : IBlogService
{
    private const string InvalidUserError = "Incorrect user id";
    private const string AlreadyExistsError = "This blog already exists";
    private const string CreateError = "Error while creating blog";
    private const string NotFoundError = "Blog not found";
    private const string UpdateError = "Error while updating blog";
    
    public async Task<Response<Guid>> Create(CreateBlogModel model)
    {
        if (!Guid.TryParse(model.UserId, out Guid userId))
        {
            return Response<Guid>.Error(InvalidUserError);
        }
        
        BlogEntity? entity = await repository.GetBy(blog => blog.Title == model.Title &&
                                                            blog.UserId == userId);

        if (entity != null)
        {
            return Response<Guid>.Error(AlreadyExistsError);
        }

        entity = new BlogEntity()
        {
            Title = model.Title,
            Id = Guid.NewGuid(),
            IsPublic = model.IsPublic,
            UserId = userId,
            CreatedDate = DateTimeOffset.Now.ToUniversalTime()
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

    public async Task<List<BlogEntity>> GetAll() => 
        await repository.GetAll()
            .Include(blog => blog.Posts)
            .Include(blog => blog.User)
            .ToListAsync();

    public async Task<Response<BlogEntity>> GetById(Guid id)
    {
        BlogEntity? entity = await repository.GetAll()
            .Include(blog => blog.User)
            .Include(blog => blog.Posts)
            .ThenInclude(post => post.Comments)
            .ThenInclude(comment => comment.User)
            .FirstOrDefaultAsync(blog => blog.Id == id);
            
        if (entity == null)
        {
            return Response<BlogEntity>.Error(NotFoundError);
        }

        return Response<BlogEntity>.OK(entity);
    }

    public async Task<Response> ChangePublic(ChangePublicModel model)
    {
        if (!Guid.TryParse(model.Id, out Guid id))
        {
            return Response.Error(NotFoundError);
        }

        BlogEntity? entity = await repository.GetById(id);
        if (entity == null)
        {
            return Response.Error(NotFoundError);
        }
        
        entity.IsPublic = model.IsPublic;

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