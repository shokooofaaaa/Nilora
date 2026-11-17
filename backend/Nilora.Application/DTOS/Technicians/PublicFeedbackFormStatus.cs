using System;

namespace Nilora.Application.DTOs.Technicians;

public enum PublicFeedbackFormStatus
{
    Success = 1,
    InvalidToken = 2,
    NotFound = 3,
    FeedbackDisabled = 4
}
