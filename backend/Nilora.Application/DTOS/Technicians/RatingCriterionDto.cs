using System;
using System.Collections.Generic;

namespace Nilora.Application.DTOs.Technicians;

public class RatingCriterionDto
{
    public string Key { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public bool Required { get; set; } = true;

    public int MinValue { get; set; } = 1;

    public int MaxValue { get; set; } = 5;
}
