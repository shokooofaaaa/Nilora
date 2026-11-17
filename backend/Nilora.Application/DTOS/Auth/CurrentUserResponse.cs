using Nilora.Domain.Enums;

namespace Nilora.Application.DTOS.Auth
{
    public record CurrentUserResponse
    {


        public int UserId { get; init; }



        public string PhoneNumber { get; init; } = string.Empty;

        public UserRole Role { get; init; }

        
        public string? FirstName { get; init; } 

       
        public string? LastName { get; init; }



    }
}