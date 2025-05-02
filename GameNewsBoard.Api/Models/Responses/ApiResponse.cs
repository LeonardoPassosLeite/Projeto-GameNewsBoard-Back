namespace GameNewsBoard.Api.Models.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public ApiResponse() { }

    public ApiResponse(string message, T? data = default)
    {
        Message = message;
        Data = data;
    }
}