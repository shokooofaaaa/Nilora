
using System.ComponentModel.DataAnnotations;

namespace Nilora.Application.DTOS.Auth
{
    public record  RegisterRequest
    {

        [Required]
        [StringLength(15, MinimumLength = 11)]
        [Phone]
        public string PhoneNumber { get; init; }


        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; init; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; init; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; init; }


    }
}
