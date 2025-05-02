namespace GameNewsBoard.Api.Models.Responses;

public class ApiErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; }
    public string? Detail { get; set; }

    public ApiErrorResponse(string message, string? detail = null)
    {
        Message = message;
        Detail = detail;
    }
}
