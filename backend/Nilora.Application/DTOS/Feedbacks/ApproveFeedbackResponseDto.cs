using System.Text.Json.Serialization;

namespace Nilora.Application.DTOS.Feedbacks
{
    public sealed class ApproveFeedbackResponseDto
    {
        [JsonPropertyName("success")]
        public bool Success { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; } = string.Empty;

        [JsonPropertyName("feedback")]
        public ApprovedFeedbackSummaryDto Feedback { get; init; } = new();
    }
}
