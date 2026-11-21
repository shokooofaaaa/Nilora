using System.Text.Json.Serialization;

namespace Nilora.Application.DTOS.Feedbacks
{
    public sealed class PendingFeedbackRatingsDto
    {
        [JsonPropertyName("quality")]
        public int Quality { get; init; }

        [JsonPropertyName("hygiene")]
        public int Hygiene { get; init; }

        [JsonPropertyName("durability")]
        public int Durability { get; init; }

        [JsonPropertyName("behavior")]
        public int Behavior { get; init; }
    }
}
