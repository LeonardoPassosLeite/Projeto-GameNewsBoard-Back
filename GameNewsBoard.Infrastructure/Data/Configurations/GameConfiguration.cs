using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(g => g.Platform)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.CoverImage)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(g => g.Rating)
            .IsRequired();

        builder.Property(g => g.Released)
            .IsRequired()
            .HasColumnType("timestamp with time zone");  
    }
}