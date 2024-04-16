using AuthService.Application.Services;
using AuthService.Domain.Interfaces;

namespace AuthService.Configuration;

public static class ServiceConfiguration
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<AccountService>();
        services.AddScoped<ProfileService>();
        services.AddScoped<TokenService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IAccountService, AccountService>();
    }
}