﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhotoAlbum.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoAlbum.Backend.Dal.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(nameof(Comment));

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Text).IsRequired();

            builder.Property(c => c.Date).IsRequired();

            builder.HasOne(c => c.Image)
                .WithMany(i => i.Comments);

            builder.HasOne(c => c.Commenter)
                .WithMany(u => u.Comments);
        }
    }
}
