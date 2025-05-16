using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Domain.Entities
{
    public class GameStatus
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public int GameId { get; private set; }
        public Game Game { get; private set; } = null!;
        public Status Status { get; private set; }

        private GameStatus() { }

        private GameStatus(Guid userId, int gameId, Status status)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            GameId = gameId;
            Status = status;
        }

        public static GameStatus Create(Guid userId, int gameId, Status status)
        {
            return new GameStatus(userId, gameId, status);
        }

        public void UpdateStatus(Status newStatus)
        {
            if (Status != newStatus)
                Status = newStatus;
        }
    }
}
