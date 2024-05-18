using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Repository;

namespace EnrollmentService.Domain.UseCase;

public class CreateApplicantUseCase(ApplicantRepository applicantRepository)
{
    public async Task Execute(Applicant applicant)
    {
        await applicantRepository.CreateApplicant(applicant);
    }
}