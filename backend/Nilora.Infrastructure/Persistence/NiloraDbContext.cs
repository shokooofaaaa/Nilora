using Microsoft.EntityFrameworkCore;
using Nilora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Persistence
{
    public class NiloraDbContext : DbContext
    {
        public NiloraDbContext(
            DbContextOptions<NiloraDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Technician> Technicians => Set<Technician>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<TechnicianSkill> TechnicianSkills => Set<TechnicianSkill>();
        public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();
        public DbSet<PortfolioMedia> PortfolioMedia => Set<PortfolioMedia>();
        public DbSet<Feedback> Feedbacks => Set<Feedback>();
        public DbSet<FeedbackMedia> FeedbackMedias => Set<FeedbackMedia>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(NiloraDbContext).Assembly);
        }
    }
}