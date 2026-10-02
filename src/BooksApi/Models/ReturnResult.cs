namespace BooksApi.Models;

public class ReturnResult<T>
{
    public bool IsSuccess { get; set; } = true;
    public T? Result { get; set; }
    public List<string> ErrorMessage { get; set; } = new();

    public static ReturnResult<T> Success(T result)
    {
        return new ReturnResult<T>
        {
            IsSuccess = true,
            Result = result,
            ErrorMessage = new List<string>()
        };
    }

    public static ReturnResult<T> Failure(params string[] errors)
    {
        return new ReturnResult<T>
        {
            IsSuccess = false,
            Result = default,
            ErrorMessage = errors.ToList()
        };
    }
}
