using Microsoft.EntityFrameworkCore;
using Nilora.Application.DTOS.Review;
using Nilora.Application.Interfaces;
using Nilora.Domain.Enums;
using Nilora.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly NiloraDbContext _context;

        public ReviewService(NiloraDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<RecentReviewDto>> GetRecentReviewsAsync(
            int technicianId,
            CancellationToken ct)
        {
            return await _context.Reviews
                .AsNoTracking() 
                .Where(x => x.TechnicianId == technicianId
                         && x.Status == ReviewStatus.Pending)
                .OrderByDescending(x => x.CreatedAt) 
                .Select(r => new RecentReviewDto(
                    r.Id, r.FirstName, r.LastName, r.Rating,
                    r.Comment, r.Status, r.CreatedAt))
                .ToListAsync(ct);
        }
    }
}
