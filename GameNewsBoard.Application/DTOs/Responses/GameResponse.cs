namespace GameNewsBoard.Application.Responses.DTOs;
public class GameResponse
{
    public string Title { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Released { get; set; } = string.Empty;
}