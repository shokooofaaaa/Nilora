using System;
using System.Collections.Generic;
using System.Text;


namespace Nilora.Application.DTOs.Dashboard;

public record DashboardOverviewDto(
    int TotalReviews,
    double AverageRating,
    int ApprovedReviews,
    int PendingReviews
);

