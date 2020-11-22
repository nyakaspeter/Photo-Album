using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable(nameof(Tag));

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Text).IsRequired();

            builder.HasOne(t => t.Image)
                .WithMany(i => i.Tags)
                .IsRequired();
        }
    }
}
