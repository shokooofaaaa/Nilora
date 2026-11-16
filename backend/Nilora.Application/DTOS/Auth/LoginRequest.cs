using System.ComponentModel.DataAnnotations;

namespace Nilora.Application.DTOS.Auth
{
    public record LoginRequest
    {
        [Required]
        [StringLength(15, MinimumLength = 11)]
        [Phone]
        public string PhoneNumber { get; init; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; init; } = string.Empty;
    }
}