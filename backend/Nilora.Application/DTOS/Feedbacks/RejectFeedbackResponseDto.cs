using System.Text.Json.Serialization;

namespace Nilora.Application.DTOS.Feedbacks
{
    public sealed class RejectFeedbackResponseDto
    {
        [JsonPropertyName("success")]
        public bool Success { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; } = string.Empty;

        [JsonPropertyName("feedback")]
        public RejectedFeedbackSummaryDto Feedback { get; init; } = new();
    }
}
