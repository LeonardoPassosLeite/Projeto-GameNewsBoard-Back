namespace GameNewsBoard.Application.Responses.DTOs.Responses
{
    public class UserProfileResponse
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;

        public List<TierListResponse> Tiers { get; set; } = new(); 
    }
}