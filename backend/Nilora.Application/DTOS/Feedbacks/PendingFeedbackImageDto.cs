using System.Text.Json.Serialization;

namespace Nilora.Application.DTOS.Feedbacks
{
    public sealed class PendingFeedbackImageDto
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; } = string.Empty;
    }
}
