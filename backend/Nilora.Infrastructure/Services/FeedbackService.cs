using Microsoft.EntityFrameworkCore;
using Nilora.Application.DTOS.Feedbacks;
using Nilora.Application.Interfaces;
using Nilora.Domain.Entities;
using Nilora.Domain.Enums;
using Nilora.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Nilora.Infrastructure.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly NiloraDbContext _context;

        public FeedbackService(NiloraDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<RecentFeedbackDto>> GetRecentFeedbacksAsync(
            int technicianId,
            CancellationToken ct)
        {
            return await _context.Feedbacks
                .AsNoTracking()
                .Where(x => x.TechnicianId == technicianId
                         && x.Status == ReviewStatus.Pending)
                .OrderByDescending(x => x.CreatedAt)
                .Select(r => new RecentFeedbackDto(
                    r.Id, r.Rating, r.Comment,
                    r.Status, r.CreatedAt))
                .ToListAsync(ct);
        }
        public async Task<int> CreateFeedbackAsync(string qrCodeToken, string clientIdentifier,
            CreateFeedbackRequestDto request,  CancellationToken ct)
                      
        {
         
            var technician = await _context.Technicians
                .FirstOrDefaultAsync(
                    x => x.QRCodeToken == qrCodeToken,
                    ct);

            if (technician is null)
                throw new KeyNotFoundException("تکنسین موردنظر پیدا نشد.");

            if (!technician.IsActive)
                throw new UnauthorizedAccessException("این تکنسین در حال حاضر غیرفعال است.");

            if (!Guid.TryParseExact(clientIdentifier, "D", out var clientGuid))
                throw new ArgumentException("شناسه مشتری معتبر نیست.");

            // تبدیل شناسه مشتری به SHA-256
            var identifierBytes = Encoding.UTF8.GetBytes(clientGuid.ToString("D"));
            var hashBytes = SHA256.HashData(identifierBytes);
            var clientIdentifierHash = Convert.ToHexString(hashBytes).ToLowerInvariant();

            // شروع تراکنش با سطح ایزولیشن Serializable
            await using var transaction = await _context.Database
                .BeginTransactionAsync(IsolationLevel.Serializable, ct);

            // بررسی محدودیت ثبت بازخورد در ۳۰ روز گذشته
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var hasRecentFeedback = await _context.Feedbacks
                .AnyAsync(x =>
                    x.TechnicianId == technician.Id &&
                    x.ClientIdentifierHash == clientIdentifierHash &&
                    x.CreatedAt > thirtyDaysAgo,
                    ct);

            if (hasRecentFeedback)
                throw new InvalidOperationException(
                    "شما در ۳۰ روز گذشته برای این تکنسین بازخورد ثبت کرده‌اید.");

            // ساخت بازخورد
            var feedback = new Feedback(
                technician.Id,
                clientIdentifierHash,
                request.QualityRating,
                request.HygieneRating,
                request.DurabilityRating,
                request.BehaviorRating,
                request.Rating,
                request.Comment);

            // ذخیره در دیتابیس
            await _context.Feedbacks.AddAsync(feedback, ct);
            await _context.SaveChangesAsync(ct);

            // ثبت نهایی تراکنش
            await transaction.CommitAsync(ct);

            return feedback.Id;
        }
    }
}
