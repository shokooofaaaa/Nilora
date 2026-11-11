using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nilora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Persistence.Configurations
{
    public class PortfolioMediaConfiguration
        : IEntityTypeConfiguration<PortfolioMedia>
    {
        public void Configure(EntityTypeBuilder<PortfolioMedia> builder)
        {
            builder.ToTable("PortfolioMedia");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.DisplayOrder)
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.PortfolioItemId,
                x.DisplayOrder
            });

            builder.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_PortfolioMedia_DisplayOrder",
                    "[DisplayOrder] >= 0");
            });
        }
    }
}
