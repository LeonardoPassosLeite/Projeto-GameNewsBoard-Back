namespace GameNewsBoard.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Username { get; private set; } = string.Empty;

        public string PasswordHash { get; private set; } = string.Empty;

        private User() { }

        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}
