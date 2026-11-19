using System.Text.Json.Serialization;

namespace Nilora.Application.DTOs.Dashboard;

public sealed class DashboardFeedbackDetailDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("rating")]
    public int Rating { get; init; }

    [JsonPropertyName("qualityRating")]
    public int QualityRating { get; init; }

    [JsonPropertyName("hygieneRating")]
    public int HygieneRating { get; init; }

    [JsonPropertyName("durabilityRating")]
    public int DurabilityRating { get; init; }

    [JsonPropertyName("behaviorRating")]
    public int BehaviorRating { get; init; }

    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("reviewedAt")]
    public DateTime? ReviewedAt { get; init; }
}
