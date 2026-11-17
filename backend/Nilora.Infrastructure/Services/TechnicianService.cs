using Microsoft.EntityFrameworkCore;
using Nilora.Application.DTOs.Technicians;
using Nilora.Application.Interfaces;
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

        public async Task<PublicTechnicianProfileDto?> GetPublicProfileByTokenAsync(
            string token,
            CancellationToken ct)
        {
            if (!IsValidToken(token))
                return null;

            string normalizedToken = token.Trim();

            var technician = await _context.Technicians
                .AsNoTracking()
                .Where(t => t.QRCodeToken == normalizedToken
                         && t.IsActive
                         && t.User.IsActive)
                .Select(t => new
                {
                    t.Id,
                    t.QRCodeToken,
                    t.FirstName,
                    t.LastName,
                    t.ProfileImagePath,
                    t.Bio
                })
                .FirstOrDefaultAsync(ct);

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

            return new PublicTechnicianProfileDto
            {
                TechnicianId = technician.Id,
                QrCodeToken = technician.QRCodeToken,
                DisplayName = BuildDisplayName(
                    technician.FirstName,
                    technician.LastName),
                ProfileImageUrl = ToPublicUrl(technician.ProfileImagePath),
                Bio = technician.Bio,
                Skills = skillsAndServices.Skills,
                Services = skillsAndServices.Services,
                PortfolioItems = portfolioItems,
                FeedbackSummary = feedbackSummary
            };
        }

        private static bool IsValidToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            string trimmedToken = token.Trim();

            if (trimmedToken.Length > MaxTokenLength)
                return false;

            return TokenPattern.IsMatch(trimmedToken);
        }

        private static string BuildDisplayName(string firstName, string lastName)
        {
            return string.Join(
                ' ',
                new[] { firstName, lastName }
                    .Where(part => !string.IsNullOrWhiteSpace(part)))
                .Trim();
        }

        /// <summary>
        /// مهارت‌های عمومی تکنسین. سرویس‌ها از همان داده‌ها ساخته می‌شوند،
        /// زیرا در مدل دامنه فعلی موجودیت Service وجود ندارد و
        /// TechnicianSkill تنها منبع مهارت/خدمت است.
        /// </summary>
        private async Task<(List<SkillDto> Skills, List<ServiceDto> Services)>
            GetPublicSkillsAndServicesAsync(int technicianId, CancellationToken ct)
        {
            var publicSkills = await _context.TechnicianSkills
                .AsNoTracking()
                .Where(ts => ts.TechnicianId == technicianId
                          && ts.Skill.IsActive)
                .OrderBy(ts => ts.Skill.DisplayOrder)
                .ThenBy(ts => ts.Skill.Title)
                .Select(ts => new
                {
                    ts.SkillId,
                    ts.Skill.Title
                })
                .ToListAsync(ct);

            var skills = publicSkills
                .Select(skill => new SkillDto
                {
                    Id = skill.SkillId,
                    Name = skill.Title
                })
                .ToList();

            var services = publicSkills
                .Select(skill => new ServiceDto
                {
                    Id = skill.SkillId,
                    Name = skill.Title
                })
                .ToList();

            return (skills, services);
        }

        private async Task<List<PortfolioItemDto>> GetPublicPortfolioItemsAsync(
            int technicianId,
            CancellationToken ct)
        {
            var portfolioItems = await _context.PortfolioItems
                .AsNoTracking()
                .Where(item => item.TechnicianId == technicianId
                            && item.IsPublished)
                .OrderByDescending(item => item.CreatedAt)
                .Select(item => new
                {
                    item.Id,
                    item.Title,
                    item.Description,
                    Media = item.Media
                        .Select(media => new
                        {
                            media.Id,
                            media.FilePath,
                            media.DisplayOrder
                        })
                        .ToList()
                })
                .ToListAsync(ct);

            return portfolioItems
                .Select(item => new PortfolioItemDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Media = item.Media
                        .OrderBy(media => media.DisplayOrder)
                        .ThenBy(media => media.Id)
                        .Where(media => !string.IsNullOrWhiteSpace(media.FilePath))
                        .Select(media => new PortfolioMediaDto
                        {
                            Id = media.Id,
                            MediaUrl = ToPublicUrl(media.FilePath) ?? string.Empty,
                            MediaType = ResolvePublicMediaType(media.FilePath)
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
        }

        /// <summary>
        /// فایل ذخیره‌شده در دیتابیس به آدرس قابل استفاده در فرانت‌اند تبدیل می‌شود.
        /// مسیر فیزیکی سرور (مثل C:\inetpub\wwwroot\...) هرگز در خروجی قرار نمی‌گیرد.
        /// </summary>
        private static string? ToPublicUrl(string? storedPath)
        {
            if (string.IsNullOrWhiteSpace(storedPath))
                return null;

            string path = storedPath.Trim().Replace('\\', '/');

            if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }

            const string webRootMarker = "wwwroot/";
            int webRootIndex = path.IndexOf(
                webRootMarker,
                StringComparison.OrdinalIgnoreCase);

            if (webRootIndex >= 0)
            {
                path = path[(webRootIndex + webRootMarker.Length)..];
            }
            else
            {
                int driveIndex = path.IndexOf(":/", StringComparison.Ordinal);

                if (driveIndex >= 0)
                    path = path[(driveIndex + 2)..];
            }

            path = path.Trim('/');

            return path.Length == 0 ? null : "/" + path;
        }

        /// <summary>
        /// نوع رسانه از پسوند فایل استنتاج می‌شود، چون PortfolioMedia
        /// در دامنه فعلی فیلد MediaType ندارد.
        /// </summary>
        private static string ResolvePublicMediaType(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.Empty;

            string extension = Path.GetExtension(filePath).TrimStart('.');

            if (extension.Length == 0)
                return string.Empty;

            if (VideoExtensions.Contains(extension))
                return MediaType.Video.ToString();

            if (ImageExtensions.Contains(extension))
                return MediaType.Image.ToString();

            return string.Empty;
        }
    }
}
