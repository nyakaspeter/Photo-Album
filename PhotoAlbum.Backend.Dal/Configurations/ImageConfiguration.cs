using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable(nameof(Image));

            builder.HasKey(i => i.Id);

            builder.Property(i => i.FileName).IsRequired();

            builder.HasOne(i => i.Uploader)
                .WithMany(u => u.Images)
                .IsRequired();

            builder.HasOne(i => i.Album)
                .WithMany(a => a.Images)
                .IsRequired();
        }
    }
}
