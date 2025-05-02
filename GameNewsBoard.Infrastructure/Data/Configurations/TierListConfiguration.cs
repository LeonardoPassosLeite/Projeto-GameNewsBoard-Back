using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TierListConfiguration : IEntityTypeConfiguration<TierList>
{
       public void Configure(EntityTypeBuilder<TierList> builder)
       {
              builder.HasKey(t => t.Id);

              builder.Property(t => t.Title)
                     .IsRequired()
                     .HasMaxLength(255);

              builder.Property(t => t.ImageUrl)
                     .HasMaxLength(500);

              builder.HasOne(t => t.User)
                     .WithMany()
                     .HasForeignKey(t => t.UserId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.HasMany(t => t.Entries)
                     .WithOne(e => e.TierList)
                     .HasForeignKey(e => e.TierListId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}