using Nilora.Application.DTOS.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Application.Interfaces
{
    public interface IReviewService
    {
        Task<IReadOnlyList<RecentReviewDto>> GetRecentReviewsAsync(int technicianId,CancellationToken ct);
    }
}
