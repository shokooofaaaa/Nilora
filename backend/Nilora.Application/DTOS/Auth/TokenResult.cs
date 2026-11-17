

namespace Nilora.Application.DTOS.Auth
{
    public record TokenResult
    {
        public string Token { get; init; } = string.Empty;

        public DateTime ExpiresAtUtc { get; init; }
    }
}