using Nilora.Application.DTOs.Technicians;
using Nilora.Application.DTOS.Technicians;

namespace Nilora.Application.Interfaces
{
    public interface ITechnicianService
    {
        Task<TechnicianProfileResponse?> GetCurrentTechnicianAsync(int userId,

           CancellationToken cancellationToken = default);

        Task<PublicTechnicianProfileDto?> GetPublicProfileByTokenAsync(
            string token,
            CancellationToken cancellationToken = default);

        Task<PublicFeedbackFormResult> GetPublicFeedbackFormAsync(
            string qrCodeToken,
            CancellationToken cancellationToken = default);
    }
}
