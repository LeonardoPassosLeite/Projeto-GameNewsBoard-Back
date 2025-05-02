using GameNewsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameNewsBoard.Infrastructure.Data.Configurations
{
    public class UploadedImageConfiguration : IEntityTypeConfiguration<UploadedImage>
    {
        public void Configure(EntityTypeBuilder<UploadedImage> builder)
        {
            builder.ToTable("UploadedImages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.UploadedAt)
                .IsRequired();

            builder.Property(x => x.IsUsed)
                .IsRequired();
        }
    }
}