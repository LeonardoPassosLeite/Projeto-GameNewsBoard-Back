namespace GameNewsBoard.Domain.Entities
{
    public class TierList
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? ImageUrl { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        public List<TierListEntry> Entries { get; private set; } = new();

        private TierList() { }

        public TierList(Guid userId, string? title = null, string? imageUrl = null)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            UpdateInfo(title, imageUrl);
        }

        public void UpdateInfo(string? newTitle = null, string? newImageUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(newTitle))
                Title = newTitle;

            if (newImageUrl is not null)
                ImageUrl = newImageUrl;
        }

        public void AddGameToTier(int gameId, TierLevel tier)
        {
            if (Entries.Any(e => e.GameId == gameId))
                throw new InvalidOperationException("Esse jogo já está no ranking.");

            Entries.Add(TierListEntry.Create(gameId, tier, this.Id));
        }

        public void UpdateGameTier(int gameId, TierLevel newTier)
        {
            var entry = Entries.FirstOrDefault(e => e.GameId == gameId);
            if (entry == null)
                throw new InvalidOperationException("Jogo não encontrado no ranking.");

            entry.UpdateTier(newTier);
        }

        public void RemoveGameFromTier(int gameId)
        {
            var entry = Entries.FirstOrDefault(e => e.GameId == gameId);
            if (entry != null)
                Entries.Remove(entry);
        }
    }
}
