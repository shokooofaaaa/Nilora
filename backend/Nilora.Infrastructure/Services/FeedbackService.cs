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
    }
}
