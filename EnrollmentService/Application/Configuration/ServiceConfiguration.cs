using EnrollmentService.Data.Repository;
using EnrollmentService.Domain.Repository;
using EnrollmentService.Domain.Service;
using EnrollmentService.Domain.UseCase;
using EnrollmentService.Presentation.Service;

namespace EnrollmentService.Application.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureApplicantServices(this IServiceCollection services)
    {
        services.AddScoped<CreateApplicantUseCase>();
        services.AddScoped<ApplicantRepositoryImpl>();
        services.AddScoped<ApplicantRepository, ApplicantRepositoryImpl>();
        services.AddScoped<AdmissionService>();
        services.AddScoped<IAdmissionService, AdmissionService>();
    }
}