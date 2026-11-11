using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nilora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Persistence.Configurations
{
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Title)
                .IsUnique();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.DisplayOrder)
                .IsRequired();

            builder.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Skills_DisplayOrder",
                    "[DisplayOrder] >= 0");
            });
        }
    }
}
