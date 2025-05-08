namespace GameNewsBoard.Domain.Entities
{
    public class TierList
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;

        public string? ImageUrl { get; private set; }
        public Guid? ImageId { get; private set; }
        public UploadedImage? Image { get; set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        public List<TierListEntry> Entries { get; private set; } = new();

        private TierList() { }

        public static TierList Create(Guid userId, string title, Guid? imageId = null, string? imageUrl = null)
        {
            return new TierList
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = title,
                ImageId = imageId,
                ImageUrl = imageUrl,
            };
        }

        public void UpdateInfo(string? newTitle = null, string? newImageUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(newTitle))
                Title = newTitle;

            if (newImageUrl is not null)
                ImageUrl = newImageUrl;
        }

        public void RemoveGameFromTier(int gameId)
        {
            var entry = Entries.FirstOrDefault(e => e.GameId == gameId);
            if (entry != null)
                Entries.Remove(entry);
        }
    }
}
