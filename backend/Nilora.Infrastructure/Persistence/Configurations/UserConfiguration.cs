using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nilora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.PhoneNumber)
                .IsUnique();

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.IsPhoneNumberVerified)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.LastLoginAt)
                .IsRequired(false);
        }
    }
}
