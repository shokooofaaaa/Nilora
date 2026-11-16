using Nilora.Application.DTOS.Auth;
using Nilora.Domain.Entities;


namespace Nilora.Application.Interfaces
{
   public interface ITokenService
    {
        global::Nilora.Application.DTOS.Auth.TokenResult GenerateToken(User user);
    }
}
