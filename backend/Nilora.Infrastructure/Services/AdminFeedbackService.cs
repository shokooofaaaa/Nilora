using Microsoft.EntityFrameworkCore;
using Nilora.Application.DTOS.Feedbacks;
using Nilora.Application.DTOs.Pagination;
using Nilora.Application.Interfaces;
using Nilora.Domain.Entities;
using Nilora.Domain.Enums;
using Nilora.Infrastructure.Persistence;
using System;

namespace Nilora.Infrastructure.Services
{
    public class AdminFeedbackService : IAdminFeedbackService
    {
        private readonly NiloraDbContext _context;

        public AdminFeedbackService(NiloraDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// دریافت لیست بازخوردهای در حال بررسی (Pending) برای مدیران، صفحه‌بندی شده.
        /// </summary>
        public async Task<PendingFeedbackListResponseDto> GetPendingFeedbacksAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var pendingFeedbacks = _context.Feedbacks
                .AsNoTracking()
                .Where(f => f.Status == ReviewStatus.Pending && !f.IsDeleted);

            var totalCount = await pendingFeedbacks
                .CountAsync(cancellationToken);

            var items = await pendingFeedbacks
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new PendingFeedbackItemDto
                {
                    Id = f.Id,
                    TechnicianId = f.TechnicianId,
                    TechnicianName = f.Technician.FirstName + " " + f.Technician.LastName,
                    QRCodeToken = f.Technician.QRCodeToken,
                    Ratings = new PendingFeedbackRatingsDto
                    {
                        Quality = f.QualityRating,
                        Hygiene = f.HygieneRating,
                        Durability = f.DurabilityRating,
                        Behavior = f.BehaviorRating
                    },
                    OverallRating = f.Rating,
                    Comment = f.Comment,
                    Images = f.Media
                        .Select(media => new PendingFeedbackImageDto
                        {
                            Id = media.Id,
                            Url = media.FilePath
                        })
                        .ToList(),
                    Status = ReviewStatus.Pending.ToString(),
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PendingFeedbackListResponseDto
            {
                Items = items,
                Pagination = new PaginationMetadataDto
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                }
            };
        }
    }
}
