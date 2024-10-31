namespace BlogApplicaiton.Services.UserServices;

public interface IUserContext
{
    Guid? Id { get; }
    string? Login { get; }
    bool? IsAuthenticated { get; }
}