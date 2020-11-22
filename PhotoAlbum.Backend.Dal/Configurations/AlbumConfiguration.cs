using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Configurations
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable(nameof(Album));

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name).IsRequired();

            builder.HasOne(a => a.Creator)
                .WithMany(u => u.CreatedAlbums);
        }
    }
}
