using AuthService.Application.Interfaces;
using AuthService.Application.Services;

namespace AuthService.Configuration;

public static class ServiceConfiguration
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddScoped<AccountService>();
        services.AddScoped<IAccountService, AccountService>();
    }
}