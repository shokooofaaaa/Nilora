using System;
using System.Collections.Generic;
using System.Text;


using Nilora.Application.DTOs.Dashboard;
using Nilora.Domain.Enums;

namespace Nilora.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardOverviewDto?> GetOverviewAsync(
        int userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RatingDistributionDto>> GetRatingDistributionAsync(
        int technicianId,
        CancellationToken cancellationToken = default);

    Task<DashboardFeedbackListDto?> GetFeedbacksAsync(
        int userId,
        int page,
        int pageSize,
        ReviewStatus? status,
        CancellationToken cancellationToken = default);
}
