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
        private static readonly Regex TokenPattern =
            new(
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

        public async Task<TechnicianProfileResponse?> GetCurrentTechnicianAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            var technician = await _context.Technicians
                .AsNoTracking()
                .Include(x => x.Skills)
                .ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(
                    x => x.UserId == userId,
                    cancellationToken);

            if (technician is null)
                return null;

            return new TechnicianProfileResponse
            {
                FirstName = technician.FirstName,
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
            };
        }

        /// <summary>
        /// دریافت پروفایل عمومی تکنسین بر اساس توکن QR (API 06).
        /// </summary>
        public async Task<PublicTechnicianProfileDto?> GetPublicProfileByTokenAsync(
            string token,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            string normalizedToken = token.Trim();

            var technician = await _context.Technicians
                .AsNoTracking()
                .Where(x => x.QRCodeToken == normalizedToken
                         && x.IsActive
                         && x.User.IsActive)
                .Select(x => new
                {
                    x.Id,
                    x.QRCodeToken,
                    x.FirstName,
                    x.LastName,
                    x.ProfileImagePath,
                    x.Bio
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (technician is null)
                return null;

            var skillsAndServices = await GetPublicSkillsAndServicesAsync(
                technician.Id,
                cancellationToken);

            var portfolioItems = await GetPublicPortfolioItemsAsync(
                technician.Id,
                cancellationToken);

            var feedbackSummary = await GetFeedbackSummaryAsync(
                technician.Id,
                cancellationToken);

            return new PublicTechnicianProfileDto()
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

        public async Task<PublicFeedbackFormResult> GetPublicFeedbackFormAsync(
            string qrCodeToken,
            CancellationToken cancellationToken = default)
        {
            // مرحله ۱: اعتبارسنجی قالب توکن
            if (!IsValidToken(qrCodeToken))
                return PublicFeedbackFormResult.InvalidToken(
                    "توکن QR ارسال‌شده معتبر نیست.");

            string normalizedToken = qrCodeToken.Trim();

            // مرحله ۲: یافتن تکنسین همراه با کاربر مرتبط
            var technician = await _context.Technicians
                .AsNoTracking()
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.QRCodeToken == normalizedToken,
                    cancellationToken);

            // مرحله ۳: بررسی وجود و فعال بودن تکنسین و حساب کاربری او
            if (technician is null
                || !technician.IsActive
                || !technician.User.IsActive)
            {
                return PublicFeedbackFormResult.NotFound(
                    "تکنسین موردنظر پیدا نشد یا حساب کاربری او غیرفعال است.");
            }

            // TODO: Uncomment when FeedbackEnabled property is added to Technician entity and DB
            // if (!technician.FeedbackEnabled)
            // {
            //     return PublicFeedbackFormResult.Forbidden("Feedback submission is currently disabled for this technician.");
            // }

            var data = new PublicFeedbackFormDto
            {
                TechnicianId = technician.Id,
                QrCodeToken = technician.QRCodeToken,
                DisplayName = BuildDisplayName(
                    technician.FirstName,
                    technician.LastName),
                ProfileImageUrl = ToPublicUrl(technician.ProfileImagePath),

                // تا زمانی که فیلد FeedbackEnabled به دامنه اضافه نشود،
                // رسیدن به این نقطه یعنی ثبت بازخورد برای این تکنسین مجاز است.
                FeedbackEnabled = true,

                RatingCriteria = BuildRatingCriteria()
            };

            return PublicFeedbackFormResult.Success(data);
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
        /// معیارهای امتیازدهی فرم بازخورد، مطابق فیلدهای CreateFeedbackRequestDto.
        /// Rating (رضایت کلی) مستقل از چهار معیار دیگر است.
        /// </summary>

        private static List<RatingCriterionDto> BuildRatingCriteria()
        {
            return new List<RatingCriterionDto>
    {
        new()
        {
            Key = "qualityRating",
            Title = "کیفیت خدمات",
            Required = true,
            MinValue = 1,
            MaxValue = 5
        },
        new()
        {
            Key = "hygieneRating",
            Title = "رعایت بهداشت",
            Required = true,
            MinValue = 1,
            MaxValue = 5
        },
        new()
        {
            Key = "durabilityRating",
            Title = "ماندگاری نتیجه",
            Required = true,
            MinValue = 1,
            MaxValue = 5
        },
        new()
        {
            Key = "behaviorRating",
            Title = "رفتار و برخورد تکنسین",
            Required = true,
            MinValue = 1,
            MaxValue = 5
        },
        new()
        {
            Key = "rating",
            Title = "رضایت کلی",
            Required = true,
            MinValue = 1,
            MaxValue = 5
        }
    };
        }


        /// <summary>
        /// مسیر ذخیره‌شده در دیتابیس را به آدرس قابل استفاده در فرانت‌اند تبدیل می‌کند
        /// و مسیر فیزیکی سرور را در خروجی قرار نمی‌دهد.
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
        /// مهارت‌های فعال تکنسین. سرویس‌ها از همان داده‌ها ساخته می‌شوند،
        /// زیرا در مدل دامنه فعلی موجودیت Service وجود ندارد.
        /// </summary>
        private async Task<(List<SkillDto> Skills, List<ServiceDto> Services)>
            GetPublicSkillsAndServicesAsync(
                int technicianId,
                CancellationToken cancellationToken)
        {
            var publicSkills = await _context.TechnicianSkills
                .AsNoTracking()
                .Where(x => x.TechnicianId == technicianId
                         && x.Skill.IsActive)
                .OrderBy(x => x.Skill.DisplayOrder)
                .ThenBy(x => x.Skill.Title)
                .Select(x => new
                {
                    x.SkillId,
                    x.Skill.Title
                })
                .ToListAsync(cancellationToken);

            var skills = publicSkills
                .Select(x => new SkillDto
                {
                    Id = x.SkillId,
                    Name = x.Title
                })
                .ToList();

            var services = publicSkills
                .Select(x => new ServiceDto
                {
                    Id = x.SkillId,
                    Name = x.Title
                })
                .ToList();

            return (skills, services);
        }

        /// <summary>
        /// نمونه‌کارهای منتشرشده تکنسین همراه با رسانه‌هایشان.
        /// </summary>
        private async Task<List<PortfolioItemDto>> GetPublicPortfolioItemsAsync(
            int technicianId,
            CancellationToken cancellationToken)
        {
            var portfolioItems = await _context.PortfolioItems
                .AsNoTracking()
                .Where(x => x.TechnicianId == technicianId
                         && x.IsPublished)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Description,
                    Media = x.Media
                        .Select(media => new
                        {
                            media.Id,
                            media.FilePath,
                            media.DisplayOrder
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

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

        private async Task<FeedbackSummaryDto> GetFeedbackSummaryAsync(
            int technicianId,
            CancellationToken cancellationToken = default)
        {
            var approvedFeedbacks = _context.Feedbacks
                .AsNoTracking()
                .Where(feedback => feedback.TechnicianId == technicianId
                                && feedback.Status == ReviewStatus.Approved
                                && !feedback.IsDeleted);

            var approvedFeedbackCount = await approvedFeedbacks
                .CountAsync(cancellationToken);

            if (approvedFeedbackCount == 0)
            {
                return new FeedbackSummaryDto
                {
                    AverageRating = null,
                    ApprovedFeedbackCount = 0
                };
            }

            double averageRating = await approvedFeedbacks
                .AverageAsync(feedback => (double)feedback.Rating, cancellationToken);

            return new FeedbackSummaryDto
            {
                AverageRating = Math.Round(averageRating, 2),
                ApprovedFeedbackCount = approvedFeedbackCount
            };
        }
    }
}
