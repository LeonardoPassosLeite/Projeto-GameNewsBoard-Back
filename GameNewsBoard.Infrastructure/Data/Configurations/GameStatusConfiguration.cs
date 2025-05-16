using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GameStatusConfiguration : IEntityTypeConfiguration<GameStatus>
{
    public void Configure(EntityTypeBuilder<GameStatus> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Status)
               .IsRequired();

        builder.HasOne(s => s.User)
               .WithMany()
               .HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Game)
               .WithMany()
               .HasForeignKey(s => s.GameId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.UserId, s.GameId })
               .IsUnique();
    }
}
