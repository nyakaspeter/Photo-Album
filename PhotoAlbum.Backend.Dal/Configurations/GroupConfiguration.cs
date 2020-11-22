using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable(nameof(Group));

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name).IsRequired();

            builder.HasOne(g => g.Creator)
                .WithMany(u => u.CreatedGroups);
        }
    }
}
