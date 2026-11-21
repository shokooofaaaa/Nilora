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

        /// <summary>
        /// تأیید یک بازخورد در حال بررسی توسط مدیر.
        /// تغییر وضعیت و ثبت اطلاعات حسابرسی (AdminId و ReviewedAt) به صورت اتمیک
        /// در یک SaveChangesAsync انجام می‌شود.
        /// </summary>
        public async Task<ApproveFeedbackResult> ApproveFeedbackAsync(
            int feedbackId,
            int adminUserId,
            CancellationToken cancellationToken = default)
        {
            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(
                    f => f.Id == feedbackId && !f.IsDeleted,
                    cancellationToken);

            if (feedback is null)
                return ApproveFeedbackResult.NotFound("بازخورد موردنظر پیدا نشد.");

            if (feedback.Status != ReviewStatus.Pending)
                return ApproveFeedbackResult.Conflict(
                    "این بازخورد قبلاً بررسی شده است و امکان تأیید مجدد آن وجود ندارد.");

            // متد دامنه، وضعیت را به Approved تغییر داده و AdminId و
            // ReviewedAt (با DateTime.UtcNow) را به صورت اتمیک به‌روزرسانی می‌کند.
            feedback.Approve(adminUserId);

            await _context.SaveChangesAsync(cancellationToken);

            return ApproveFeedbackResult.Success(new ApproveFeedbackResponseDto
            {
                Success = true,
                Message = "بازخورد با موفقیت تأیید شد.",
                Feedback = new ApprovedFeedbackSummaryDto
                {
                    Id = feedback.Id,
                    Status = feedback.Status.ToString(),
                    AdminId = feedback.AdminId!.Value,
                    ReviewedAt = feedback.ReviewedAt!.Value
                }
            });
        }

        /// <summary>
        /// رد یک بازخورد در حال بررسی توسط مدیر.
        /// تغییر وضعیت و ثبت اطلاعات حسابرسی (AdminId، ReviewedAt و RejectionReason)
        /// به صورت اتمیک در یک SaveChangesAsync انجام می‌شود.
        /// </summary>
        public async Task<RejectFeedbackResult> RejectFeedbackAsync(
            int feedbackId,
            int adminUserId,
            string? reason,
            CancellationToken cancellationToken = default)
        {
            var feedback = await _context.Feedbacks
                .FirstOrDefaultAsync(
                    f => f.Id == feedbackId && !f.IsDeleted,
                    cancellationToken);

            if (feedback is null)
                return RejectFeedbackResult.NotFound("بازخورد موردنظر پیدا نشد.");

            if (feedback.Status != ReviewStatus.Pending)
                return RejectFeedbackResult.Conflict(
                    "این بازخورد قبلاً بررسی شده است و امکان رد مجدد آن وجود ندارد.");

            var normalizedReason = string.IsNullOrWhiteSpace(reason)
                ? null
                : reason.Trim();

            // متد دامنه، وضعیت را به Rejected تغییر داده و AdminId و
            // ReviewedAt (با DateTime.UtcNow) را به‌روزرسانی می‌کند.
            feedback.Reject(adminUserId);
            feedback.RejectionReason = normalizedReason;

            await _context.SaveChangesAsync(cancellationToken);

            return RejectFeedbackResult.Success(new RejectFeedbackResponseDto
            {
                Success = true,
                Message = "بازخورد با موفقیت رد شد.",
                Feedback = new RejectedFeedbackSummaryDto
                {
                    Id = feedback.Id,
                    Status = feedback.Status.ToString(),
                    AdminId = feedback.AdminId!.Value,
                    ReviewedAt = feedback.ReviewedAt!.Value,
                    RejectionReason = feedback.RejectionReason
                }
            });
        }
    }
}
