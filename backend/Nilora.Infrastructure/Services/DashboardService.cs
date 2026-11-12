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

    public async Task<DashboardOverviewDto> GetOverviewAsync(
        int technicianId,
        CancellationToken cancellationToken = default)
    {
        var reviews = _context.Reviews
            .Where(r => r.TechnicianId == technicianId);

        var totalReviews = await reviews.CountAsync(cancellationToken);

        var approvedReviews = await reviews.CountAsync(
            r => r.Status == ReviewStatus.Approved,
            cancellationToken);

        var pendingReviews = await reviews.CountAsync(
            r => r.Status == ReviewStatus.Pending,
            cancellationToken);

        var averageRating = approvedReviews == 0
            ? 0
            : await reviews
                .Where(r => r.Status == ReviewStatus.Approved)
                .AverageAsync(r => (double)r.Rating, cancellationToken);

        return new DashboardOverviewDto(
            totalReviews,
            averageRating,
            approvedReviews,
            pendingReviews);
    }

    public async Task<IReadOnlyList<RatingDistributionDto>>
        GetRatingDistributionAsync(
            int technicianId,
            CancellationToken cancellationToken = default)
    {
        var distribution = await _context.Reviews
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
}
