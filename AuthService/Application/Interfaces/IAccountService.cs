using AuthService.Domain.Entity;
using AuthService.Presentation.Models;

namespace AuthService.Application.Interfaces;

public interface IAccountService
{
   public Task<UserDTO> GetUserById(string userId);
}
