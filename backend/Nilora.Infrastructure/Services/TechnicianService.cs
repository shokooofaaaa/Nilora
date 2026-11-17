using Microsoft.EntityFrameworkCore;
using Nilora.Application.DTOs.Technicians;
using Nilora.Application.DTOS.Technicians;
using Nilora.Application.Interfaces;
using Nilora.Domain.Entities;
using Nilora.Domain.Enums;
using Nilora.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nilora.Infrastructure.Services
{
    public class TechnicianService : ITechnicianService
    {
        /// <summary>
        /// حداکثر طول مجاز توکن، مطابق TechnicianConfiguration (nvarchar(100)).
        /// </summary>
        private const int MaxTokenLength = 100;

        /// <summary>
        /// توکن QR فقط می‌تواند شامل حرف، رقم، خط تیره و زیرخط باشد.
        /// </summary>
        private static readonly Regex TokenPattern = new(
            "^[A-Za-z0-9_-]+$",
            RegexOptions.CultureInvariant);

        private static readonly HashSet<string> ImageExtensions = new(
            new[] { "jpg", "jpeg", "png", "gif", "webp", "bmp", "svg", "avif" },
            StringComparer.OrdinalIgnoreCase);

        private static readonly HashSet<string> VideoExtensions = new(
            new[] { "mp4", "webm", "mov", "avi", "mkv", "m4v" },
            StringComparer.OrdinalIgnoreCase);

        private readonly NiloraDbContext _context;

        public TechnicianService(NiloraDbContext context)
        {
            _context = context;
        }

        public async Task<TechnicianProfileResponse?> GetCurrentTechnicianAsync(int userId, CancellationToken cancellationToken = default)
        {
            if (!IsValidToken(token))
                return null;

            string normalizedToken = token.Trim();

            var technician = await _context.Technicians
                              .Include(x => x.Skills)
                            .ThenInclude(x => x.Skill)
                           .FirstOrDefaultAsync(
                            x => x.UserId == userId,
                            cancellationToken);

            if (technician is null)
                return null;

            var skillsAndServices = await GetPublicSkillsAndServicesAsync(
                technician.Id,
                ct);

            var portfolioItems = await GetPublicPortfolioItemsAsync(
                technician.Id,
                ct);

            var feedbackSummary = await GetFeedbackSummaryAsync(
                technician.Id,
                ct);

            return new PublicTechnicianProfileDto()
            {
                return null;
            }

            TechnicianProfileResponse technicianProfile =
                new TechnicianProfileResponse()
                {
                   FirstName = Technician.FirstName,
                   LastName = technician.LastName,
                   Bio = technician.Bio,
                   Address = technician.Address,
                    ProfileImagePath = technician.ProfileImagePath,
                    CoverImagePath = technician.CoverImagePath,
                    Skills = technician.Skills
                   .Select(x => new TechnicianSkillResponse
                   {
                     SkillId = x.SkillId,
                     Title = x.Skill.Title
                    })
                   .ToList()
                })
                .ToList();
        }

        private async Task<FeedbackSummaryDto> GetFeedbackSummaryAsync(
            int technicianId,
            CancellationToken ct)
        {
            var approvedFeedbacks = _context.Feedbacks
                .AsNoTracking()
                .Where(feedback => feedback.TechnicianId == technicianId
                                && feedback.Status == ReviewStatus.Approved
                                && !feedback.IsDeleted);

            var approvedFeedbackCount = await approvedFeedbacks.CountAsync(ct);

            if (approvedFeedbackCount == 0)
            {
                return new FeedbackSummaryDto
                {
                    AverageRating = null,
                    ApprovedFeedbackCount = 0
                };
            }

            double averageRating = await approvedFeedbacks
                .AverageAsync(feedback => (double)feedback.Rating, ct);

            return new FeedbackSummaryDto
            {
                AverageRating = Math.Round(averageRating, 2),
                ApprovedFeedbackCount = approvedFeedbackCount
                };
            return technicianProfile;
        }
    }
}
