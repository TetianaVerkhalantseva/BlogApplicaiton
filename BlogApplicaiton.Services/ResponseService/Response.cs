namespace BlogApplicaiton.Services.ResponseService;

public sealed class Response
{
    public string ErrorMessage { get; set; } = String.Empty;
    public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
    
    public static Response OK() => new();
    public static Response Error(string message) =>
        new()
        {
            ErrorMessage = message
        };
}

public sealed class Response<T>
{
    public T Result { get; set; }
    public string ErrorMessage { get; set; } = String.Empty;
    public bool IsError => !string.IsNullOrEmpty(ErrorMessage);

    public static Response<T> OK(T result) => new() { Result = result };
    public static Response<T> Error(string message) =>
        new()
        {
            ErrorMessage = message
        };
}