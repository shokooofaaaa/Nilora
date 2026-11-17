using System;
using System.Collections.Generic;

namespace Nilora.Application.DTOs.Technicians;

public class PublicTechnicianProfileDto
{
    public int TechnicianId { get; set; }

    public string QrCodeToken { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? ProfileImageUrl { get; set; }

    public string? Bio { get; set; }

    public List<SkillDto> Skills { get; set; } = new();

    public List<ServiceDto> Services { get; set; } = new();

    public List<PortfolioItemDto> PortfolioItems { get; set; } = new();

    public FeedbackSummaryDto FeedbackSummary { get; set; } = new();
}
