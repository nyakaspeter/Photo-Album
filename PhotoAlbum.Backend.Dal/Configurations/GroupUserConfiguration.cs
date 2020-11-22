using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Configurations
{
    public class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder.ToTable(nameof(GroupUser));

            builder.HasKey(gu => new { gu.GroupId, gu.UserId });

            builder.HasOne(gu => gu.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(gu => gu.GroupId);

            builder.HasOne(gu => gu.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(gu => gu.UserId);
        }
    }
}
