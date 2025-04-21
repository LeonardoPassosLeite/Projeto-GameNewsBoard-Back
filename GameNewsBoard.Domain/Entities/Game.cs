namespace GameNewsBoard.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;
        public double Rating { get; set; }
        public DateTimeOffset Released { get; set; }
    }
}
