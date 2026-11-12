using System;
using System.Collections.Generic;
using System.Text;


namespace Nilora.Application.DTOs.Dashboard;

public record RatingDistributionDto(
    int Rating,
    int Count
);

