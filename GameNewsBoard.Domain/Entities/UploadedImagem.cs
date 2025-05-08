namespace GameNewsBoard.Domain.Entities
{
    public class UploadedImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Url { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; } = false;

        public void ImageInUsed()
        {
            IsUsed = true;
        }
    }
}
