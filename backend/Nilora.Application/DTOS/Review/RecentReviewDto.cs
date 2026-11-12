using Nilora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Application.DTOS.Review
{
    public record RecentReviewDto(
        int Id,
        string FirstName,
        string LastName,
        int TechnicianId,
        ReviewStatus Status,
        string Comment,
        int Rating,
        DateTime CreatedAt,
        DateTime ReviewedAt,
        string? MediaUrl);
 
}
