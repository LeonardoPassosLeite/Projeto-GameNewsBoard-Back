namespace GameNewsBoard.Application.Responses.DTOs.Responses
{
    public class UserResponse
    {
        public bool Authenticated { get; set; }
        public string Username { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

        public UserResponse(bool authenticated, string username, string userId)
        {
            Authenticated = authenticated;
            Username = username;
            UserId = userId;
        }
    }
}