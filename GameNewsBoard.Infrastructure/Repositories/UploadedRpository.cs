using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameNewsBoard.Infrastructure.Repositories
{
    public class UploadedImageRepository : IUploadedImageRepository
    {
        private readonly AppDbContext _context;

        public UploadedImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(UploadedImage image)
        {
            _context.UploadedImages.Add(image);
            return Task.CompletedTask;
        }

        public Task<UploadedImage?> GetByIdAsync(Guid id)
        {
            return _context.UploadedImages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<IEnumerable<UploadedImage>> GetUnusedByUserAsync(Guid userId)
        {
            return Task.FromResult(_context.UploadedImages
                .Where(x => x.UserId == userId && !x.IsUsed)
                .AsEnumerable());
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}