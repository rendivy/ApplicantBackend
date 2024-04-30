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

    public static void AddAutoMigrations(IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<EnrollmentDatabaseContext>();
            context.Database.MigrateAsync();
        }
    }
}