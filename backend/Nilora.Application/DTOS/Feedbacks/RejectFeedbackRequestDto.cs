using System.Text.Json.Serialization;

namespace Nilora.Application.DTOS.Feedbacks
{
    public sealed class RejectFeedbackRequestDto
    {
        [JsonPropertyName("reason")]
        public string? Reason { get; init; }
    }
}
