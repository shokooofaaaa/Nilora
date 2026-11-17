using System;
using System.Collections.Generic;

namespace Nilora.Application.DTOs.Technicians;

public class PortfolioItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public List<PortfolioMediaDto> Media { get; set; } = new();
}
