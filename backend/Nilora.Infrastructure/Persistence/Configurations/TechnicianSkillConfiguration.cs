using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nilora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Persistence.Configurations
{
    public class TechnicianSkillConfiguration
      : IEntityTypeConfiguration<TechnicianSkill>
    {
        public void Configure(EntityTypeBuilder<TechnicianSkill> builder)
        {
            builder.ToTable("TechnicianSkills");

            builder.HasKey(x => new
            {
                x.TechnicianId,
                x.SkillId
            });

            builder.HasOne(x => x.Technician)
                .WithMany(x => x.Skills)
                .HasForeignKey(x => x.TechnicianId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Skill)
                .WithMany()
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
