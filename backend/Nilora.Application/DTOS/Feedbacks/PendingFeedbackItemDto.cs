using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nilora.Application.DTOS.Feedbacks
{
    public sealed class PendingFeedbackItemDto
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("technicianId")]
        public int TechnicianId { get; init; }

        [JsonPropertyName("technicianName")]
        public string TechnicianName { get; init; } = string.Empty;

        [JsonPropertyName("technicianSlug")]
        public string? QRCodeToken { get; init; }

        [JsonPropertyName("ratings")]
        public PendingFeedbackRatingsDto Ratings { get; init; } = new();

        [JsonPropertyName("overallRating")]
        public int OverallRating { get; init; }

        [JsonPropertyName("comment")]
        public string? Comment { get; init; }

        [JsonPropertyName("images")]
        public List<PendingFeedbackImageDto> Images { get; init; } = [];

        [JsonPropertyName("status")]
        public string Status { get; init; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; init; }
    }
}
