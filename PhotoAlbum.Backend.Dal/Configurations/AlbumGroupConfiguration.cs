using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Configurations
{
    public class AlbumGroupConfiguration : IEntityTypeConfiguration<AlbumGroup>
    {
        public void Configure(EntityTypeBuilder<AlbumGroup> builder)
        {
            builder.ToTable(nameof(AlbumGroup));

            builder.HasKey(ag => new { ag.AlbumId, ag.GroupId });

            builder.HasOne(ag => ag.Album)
                .WithMany(a => a.Groups)
                .HasForeignKey(ag => ag.AlbumId);

            builder.HasOne(ag => ag.Group)
                .WithMany(g => g.Albums)
                .HasForeignKey(ag => ag.GroupId);
        }
    }
}
