using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nilora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Persistence.Configurations
{


    public class TechnicianConfiguration
       : IEntityTypeConfiguration<Technician>
    {
        public void Configure(EntityTypeBuilder<Technician> builder)
        {
            builder.ToTable("Technicians");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Bio)
                .HasMaxLength(1000);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.ProfileImagePath)
                .HasMaxLength(500);

            builder.Property(x => x.CoverImagePath)
                .HasMaxLength(500);

            builder.Property(x => x.QRCodeToken)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.QRCodeToken)
                .IsUnique();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // User 1 ---- 0..1 Technician
            builder.HasOne(x => x.User)
                .WithOne(x => x.Technician)
                .HasForeignKey<Technician>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Technician 1 ---- N TechnicianSkill
            builder.HasMany(x => x.Skills)
                .WithOne(x => x.Technician)
                .HasForeignKey(x => x.TechnicianId)
                .OnDelete(DeleteBehavior.Cascade);

            // Technician 1 ---- N PortfolioItem
            builder.HasMany(x => x.PortfolioItems)
                .WithOne(x => x.Technician)
                .HasForeignKey(x => x.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            // Technician 1 ---- N Review
            builder.HasMany(x => x.Reviews)
                .WithOne(x => x.Technician)
                .HasForeignKey(x => x.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    }
