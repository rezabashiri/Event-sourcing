namespace Shared.Core.Wrapper;

public class PaginatedResult<T> : Result
{
    public PaginatedResult(List<T> data)
    {
        Data = data;
    }

    internal PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
    {
        Data = data;
        CurrentPage = page;
        Succeeded = succeeded;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public List<T> Data { get; set; }
    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public static PaginatedResult<T> Failure(List<string> messages) => new(false, default, messages);

    public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize) => new(true, data, null, count, page, pageSize);

    public static PaginatedResult<T> Fail(string message) => new(false)
    {
        Succeeded = false,
        Messages = new List<string>
        {
            message
        }
    };
}