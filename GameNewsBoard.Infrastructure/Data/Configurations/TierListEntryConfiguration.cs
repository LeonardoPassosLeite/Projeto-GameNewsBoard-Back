using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TierListEntryConfiguration : IEntityTypeConfiguration<TierListEntry>
{
       public void Configure(EntityTypeBuilder<TierListEntry> builder)
       {
              builder.HasKey(e => e.Id);

              builder.HasOne(e => e.Game)
                     .WithMany()
                     .HasForeignKey(e => e.GameId);

              builder.HasOne(e => e.TierList)
                     .WithMany(t => t.Entries)
                     .HasForeignKey(e => e.TierListId)
                     .OnDelete(DeleteBehavior.Cascade); 

              builder.Property(e => e.Tier)
                     .HasConversion<string>()
                     .IsRequired();
       }
}