using System.Text.Json.Serialization;

namespace Nilora.Application.DTOs.Pagination
{
    public sealed class PaginationMetadataDto
    {
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; init; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; init; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; init; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; init; }
    }
}
