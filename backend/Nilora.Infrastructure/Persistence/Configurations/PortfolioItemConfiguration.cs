using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nilora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Persistence.Configurations
{
    public class PortfolioItemConfiguration
       : IEntityTypeConfiguration<PortfolioItem>
    {
        public void Configure(EntityTypeBuilder<PortfolioItem> builder)
        {
            builder.ToTable("PortfolioItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            builder.Property(x => x.CoverImagePath)
                .HasMaxLength(500);

            builder.Property(x => x.IsPublished)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.TechnicianId);

            builder.HasMany(x => x.Media)
                .WithOne(x => x.PortfolioItem)
                .HasForeignKey(x => x.PortfolioItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Media)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
