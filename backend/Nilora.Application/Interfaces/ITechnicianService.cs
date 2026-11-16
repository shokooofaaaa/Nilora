using Nilora.Application.DTOS.Technicians;

namespace Nilora.Application.Interfaces
{
    public interface ITechnicianService
    {
        Task<TechnicianProfileResponse?> GetCurrentTechnicianAsync(int userId,

           CancellationToken cancellationToken = default);

    }
}
