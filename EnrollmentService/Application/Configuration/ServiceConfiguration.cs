using EnrollmentService.Application.BackgroundWorkers;
using EnrollmentService.Data.Repository;
using EnrollmentService.Domain.Repository;
using EnrollmentService.Domain.UseCase;

namespace EnrollmentService.Application.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureApplicantServices(this IServiceCollection services)
    {
        services.AddScoped<CreateApplicantUseCase>();
        services.AddScoped<ApplicantRepositoryImpl>();
        services.AddScoped<ApplicantRepository, ApplicantRepositoryImpl>();
    }
}