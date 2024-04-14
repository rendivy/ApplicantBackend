using AuthService.Domain.Entity;
using AuthService.Presentation.Models;

namespace AuthService.Application.Interfaces;

public interface IAccountService
{
    public Task<UserRequest> GetUserById(string userId);

    public Task AddRole(string currentUserId, string userId, Roles role);

    public Task<TokenResponse> Registration(RegistrationRequest registrationRequest);

    public Task<TokenResponse> Login(LoginRequest loginRequest);
}