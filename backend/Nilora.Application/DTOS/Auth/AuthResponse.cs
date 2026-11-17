
namespace Nilora.Application.DTOS.Auth
{
    public record AuthResponse
    {

        
        public int UserId { get; init; }


        
        public string Token { get; init; }=string.Empty;

        public DateTime ExpiresAtUtc {  get; init; }



    }
}
