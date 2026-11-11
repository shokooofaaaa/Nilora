using System;
using System.Collections.Generic;
using System.Text;


using Nilora.Application.DTOs.Dashboard;

namespace Nilora.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardOverviewDto> GetOverviewAsync(
        int technicianId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RatingDistributionDto>> GetRatingDistributionAsync(
        int technicianId,
        CancellationToken cancellationToken = default);
}
