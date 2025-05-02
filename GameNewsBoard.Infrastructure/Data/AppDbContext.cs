using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameNewsBoard.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TierList> TierLists { get; set; }
        public DbSet<UploadedImage> UploadedImages { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
