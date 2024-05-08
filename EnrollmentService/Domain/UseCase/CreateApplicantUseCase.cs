using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Repository;

namespace EnrollmentService.Domain.UseCase;

public class CreateApplicantUseCase(ApplicantRepository applicantRepository)
{
    private readonly ApplicantRepository _applicantRepository = applicantRepository;

    public async Task Execute(Applicant applicant)
    {
        await _applicantRepository.CreateApplicant(applicant);
    }
}