namespace GameNewsBoard.Application.DTOs.Requests
{
    public class TierListRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public Guid? ImageId { get; set; }
    }

    public class UpdateTierListRequest
    {
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
    }
}