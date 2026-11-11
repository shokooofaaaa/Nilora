using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nilora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Comment)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.ReviewedAt)
                .IsRequired(false);

            // Technician 1 ---- N Review
            builder.HasOne(x => x.Technician)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            // User 1 ---- N Review (Admin)
            builder.HasOne(x => x.Admin)
                .WithMany()
                .HasForeignKey(x => x.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review 1 ---- N ReviewMedia
            builder.HasMany(x => x.Media)
                .WithOne(x => x.Review)
                .HasForeignKey(x => x.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Media)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(x => new
            {
                x.TechnicianId,
                x.Status,
                x.CreatedAt
            });

            builder.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Reviews_Rating",
                    "[Rating] >= 1 AND [Rating] <= 5");

                table.HasCheckConstraint(
                    "CK_Reviews_Status",
                    "[Status] IN (1, 2, 3)");
            });
        }
    }
}
