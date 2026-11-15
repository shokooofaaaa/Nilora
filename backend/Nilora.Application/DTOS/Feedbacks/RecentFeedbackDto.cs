using Nilora.Domain.Enums;
using System;

namespace Nilora.Application.DTOS.Feedbacks
{
    public record RecentFeedbackDto(
        int Id,
        int Rating,
        string? Comment,
        ReviewStatus Status,
        DateTime CreatedAt);
}