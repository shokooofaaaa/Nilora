using System.Text.Json.Serialization;

namespace Nilora.Application.DTOs.Dashboard;

public sealed class DashboardFeedbackListDto
{
    [JsonPropertyName("items")]
    public IReadOnlyList<DashboardFeedbackItemDto> Items { get; init; } = [];

    [JsonPropertyName("page")]
    public int Page { get; init; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; init; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; init; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; init; }
}
