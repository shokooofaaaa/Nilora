using System;
using System.Collections.Generic;
using System.Text;


using Microsoft.EntityFrameworkCore;
using Nilora.Application.DTOs.Dashboard;
using Nilora.Application.Interfaces;
using Nilora.Domain.Enums;
using Nilora.Infrastructure.Persistence;

namespace Nilora.Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly NiloraDbContext _context;

    public DashboardService(NiloraDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardOverviewDto?> GetOverviewAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var technicianId = await _context.Technicians
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .Select(t => (int?)t.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (technicianId is null)
            return null;

        var feedbacks = _context.Feedbacks
            .AsNoTracking()
            .Where(f => f.TechnicianId == technicianId.Value && !f.IsDeleted);

        var totalFeedbacks = await feedbacks.CountAsync(cancellationToken);

        var approvedFeedbacks = await feedbacks.CountAsync(
            f => f.Status == ReviewStatus.Approved,
            cancellationToken);

        var pendingFeedbacks = await feedbacks.CountAsync(
            f => f.Status == ReviewStatus.Pending,
            cancellationToken);

        var averageRating = approvedFeedbacks == 0
            ? 0d
            : await feedbacks
                .Where(f => f.Status == ReviewStatus.Approved)
                .AverageAsync(f => (double)f.Rating, cancellationToken);

        var positiveFeedbacks = approvedFeedbacks == 0
            ? 0
            : await feedbacks.CountAsync(
                f => f.Status == ReviewStatus.Approved && f.Rating >= 4,
                cancellationToken);

        var positiveFeedbackPercentage = approvedFeedbacks == 0
            ? 0d
            : positiveFeedbacks * 100d / approvedFeedbacks;

        return new DashboardOverviewDto(
            totalFeedbacks,
            approvedFeedbacks,
            pendingFeedbacks,
            averageRating,
            positiveFeedbackPercentage);
    }

    public async Task<IReadOnlyList<RatingDistributionDto>>
        GetRatingDistributionAsync(
            int technicianId,
            CancellationToken cancellationToken = default)
    {
        var distribution = await _context.Feedbacks
            .Where(r =>
                r.TechnicianId == technicianId &&
                r.Status == ReviewStatus.Approved)
            .GroupBy(r => r.Rating)
            .Select(g => new
            {
                Rating = g.Key,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        return Enumerable.Range(1, 5)
            .Select(rating =>
            {
                var item = distribution.FirstOrDefault(
                    x => x.Rating == rating);

                return new RatingDistributionDto(
                    rating,
                    item?.Count ?? 0);
            })
            .ToList();
    }

    public async Task<DashboardFeedbackListDto?> GetFeedbacksAsync(
        int userId,
        int page,
        int pageSize,
        ReviewStatus? status,
        CancellationToken cancellationToken = default)
    {
        var technicianId = await _context.Technicians
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .Select(t => (int?)t.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (technicianId is null)
            return null;

        var feedbacks = _context.Feedbacks
            .AsNoTracking()
            .Where(f => f.TechnicianId == technicianId.Value && !f.IsDeleted);

        if (status.HasValue)
            feedbacks = feedbacks.Where(f => f.Status == status.Value);

        var totalCount = await feedbacks.CountAsync(cancellationToken);

        var items = await feedbacks
            .OrderByDescending(f => f.CreatedAt)
            .ThenByDescending(f => f.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(f => new DashboardFeedbackItemDto
            {
                Id = f.Id,
                Rating = f.Rating,
                QualityRating = f.QualityRating,
                HygieneRating = f.HygieneRating,
                DurabilityRating = f.DurabilityRating,
                BehaviorRating = f.BehaviorRating,
                Comment = f.Comment,
                Status = f.Status.ToString(),
                CreatedAt = f.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)pageSize);

        return new DashboardFeedbackListDto
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}
