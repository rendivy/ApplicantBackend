using EnrollmentService.Domain.Entity;

namespace EnrollmentService.Domain.Repository;

public interface ApplicantRepository
{
    Task CreateApplicant(Applicant applicant);
}