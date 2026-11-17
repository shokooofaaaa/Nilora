using System;
using System.Collections.Generic;

namespace Nilora.Application.DTOs.Technicians;

public class PortfolioMediaDto
{
    public int Id { get; set; }

    public string MediaUrl { get; set; } = string.Empty;

    public string MediaType { get; set; } = string.Empty;
}
