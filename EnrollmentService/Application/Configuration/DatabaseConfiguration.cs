using EnrollmentService.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentService.Application.Configuration;

public static class DatabaseConfiguration
{
    public static void AddDatabaseContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EnrollmentDatabaseContext>(
            it => it.UseNpgsql(configuration.GetConnectionString("EnrollmentDatabaseConnection")));
    }
}