using System.Text.Json.Serialization;
using Nilora.Application.DTOs.Pagination;

namespace Nilora.Application.DTOS.Feedbacks
{
    public sealed class PendingFeedbackListResponseDto
    {
        [JsonPropertyName("items")]
        public IReadOnlyList<PendingFeedbackItemDto> Items { get; init; } = [];

        [JsonPropertyName("pagination")]
        public PaginationMetadataDto Pagination { get; init; } = new();
    }
}
