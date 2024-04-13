using AuthService.Domain.Entity;
using AuthService.Presentation.Models;

namespace AuthService.Application.Interfaces;

public interface IAccountService
{
   public Task<UserDTO> GetUserById(string userId);

   public Task<TokenResponse> Registration(RegistrationRequest registrationRequest);
}
