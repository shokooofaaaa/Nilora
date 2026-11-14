using Nilora.Application.DTOS.Feedbacks;

namespace Nilora.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<IReadOnlyList<RecentFeedbackDto>> GetRecentFeedbacksAsync(
            int technicianId,
            CancellationToken ct);
    }
}