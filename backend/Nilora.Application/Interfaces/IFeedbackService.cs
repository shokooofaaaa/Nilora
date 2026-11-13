using Nilora.Application.DTOS.Review;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<IReadOnlyList<RecentFeedbackDto>> GetRecentFeedbacksAsync(int technicianId,CancellationToken ct);
    }
}
