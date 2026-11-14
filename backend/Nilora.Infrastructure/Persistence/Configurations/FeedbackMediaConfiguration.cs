using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nilora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Persistence.Configurations
{
    public class FeedbackMediaConfiguration
        : IEntityTypeConfiguration<FeedbackMedia>
    {
        public void Configure(EntityTypeBuilder<FeedbackMedia> builder)
        {
            builder.ToTable("FeedbackMedia");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FilePath)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
