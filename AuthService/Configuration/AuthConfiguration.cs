using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Configuration;

public static class AuthConfiguration
{
    public static void AddJwt(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(
            it =>
            {
                var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("AppSettings:SecretKey"));
                it.SaveToken = true;
                it.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            }
        );
    }
}