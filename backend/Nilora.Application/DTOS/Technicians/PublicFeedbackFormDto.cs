using System;
using System.Collections.Generic;

namespace Nilora.Application.DTOs.Technicians;

public class PublicFeedbackFormDto
{
    public int TechnicianId { get; set; }

    public string QrCodeToken { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? ProfileImageUrl { get; set; }

    public bool FeedbackEnabled { get; set; }

    public List<RatingCriterionDto> RatingCriteria { get; set; } = new();
}
