using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Configurations
{
    public class AlbumUserConfiguration : IEntityTypeConfiguration<AlbumUser>
    {
        public void Configure(EntityTypeBuilder<AlbumUser> builder)
        {
            builder.ToTable(nameof(AlbumUser));

            builder.HasKey(au => new { au.AlbumId, au.UserId });

            builder.HasOne(au => au.Album)
                .WithMany(a => a.Users)
                .HasForeignKey(au => au.AlbumId);

            builder.HasOne(au => au.User)
                .WithMany(u => u.Albums)
                .HasForeignKey(au => au.UserId);
        }
    }
}
