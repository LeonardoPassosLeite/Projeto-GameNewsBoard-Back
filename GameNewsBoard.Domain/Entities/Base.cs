namespace GameNewsBoard.Domain.Entities
{
    public class Base
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
    }
}
