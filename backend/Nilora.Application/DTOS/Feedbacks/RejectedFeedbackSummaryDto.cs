using System;
using System.Text.Json.Serialization;

namespace Nilora.Application.DTOS.Feedbacks
{
    public sealed class RejectedFeedbackSummaryDto
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("status")]
        public string Status { get; init; } = string.Empty;

        [JsonPropertyName("adminId")]
        public int AdminId { get; init; }

        [JsonPropertyName("reviewedAt")]
        public DateTime ReviewedAt { get; init; }

        [JsonPropertyName("rejectionReason")]
        public string? RejectionReason { get; init; }
    }
}
