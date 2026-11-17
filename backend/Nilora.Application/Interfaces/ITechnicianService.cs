using Nilora.Application.DTOs.Technicians;

namespace Nilora.Application.Interfaces
{
    public interface ITechnicianService
    {
        Task<PublicTechnicianProfileDto?> GetPublicProfileByTokenAsync(
            string token,
            CancellationToken ct);
    }
}
