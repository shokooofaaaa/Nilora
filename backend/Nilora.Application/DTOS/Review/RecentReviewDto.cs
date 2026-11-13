using Nilora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nilora.Application.DTOS.Review
{
    public record RecentReviewDto(
        int Id,
        int Rating, string? Comment,
        ReviewStatus Status, DateTime CreatedAt);

}
