using System;
using System.Collections.Generic;
using System.Text;


namespace Nilora.Application.DTOs.Dashboard;

public record DashboardOverviewDto(
    int TotalFeedbacks,
    int ApprovedFeedbacks,
    int PendingFeedbacks,
    double AverageRating,
    double PositiveFeedbackPercentage
);

