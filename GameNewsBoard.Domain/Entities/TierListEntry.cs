namespace GameNewsBoard.Domain.Entities
{
    public class TierListEntry
    {
        public Guid Id { get; private set; }
        public Guid TierListId { get; private set; }
        public TierList TierList { get; private set; } = null!;

        public int GameId { get; private set; }
        public Game Game { get; private set; } = null!;
        public TierLevel Tier { get; private set; }

        private TierListEntry() { }

        private TierListEntry(int gameId, TierLevel tier, Guid tierListId)
        {
            Id = Guid.NewGuid();
            GameId = gameId;
            Tier = tier;
            TierListId = tierListId;
        }

        public static TierListEntry Create(int gameId, TierLevel tier, Guid tierListId)
        {
            return new TierListEntry(gameId, tier, tierListId);
        }

        public void UpdateTier(TierLevel newTier)
        {
            Tier = newTier;
        }
    }
}
