using System.Threading;
using System.Threading.Tasks;

using Nilora.Application.DTOS.Feedbacks;

namespace Nilora.Application.Interfaces
{
    public interface IAdminFeedbackService
    {
        Task<PendingFeedbackListResponseDto> GetPendingFeedbacksAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<ApproveFeedbackResult> ApproveFeedbackAsync(
            int feedbackId,
            int adminUserId,
            CancellationToken cancellationToken = default);
    }
}
