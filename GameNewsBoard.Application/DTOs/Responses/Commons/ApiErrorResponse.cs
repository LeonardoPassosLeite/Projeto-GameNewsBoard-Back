namespace GameNewsBoard.Application.Responses.Commons
{
    public class ApiErrorResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}