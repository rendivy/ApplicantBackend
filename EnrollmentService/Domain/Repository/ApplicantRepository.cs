using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Entity.Stuff;

namespace EnrollmentService.Domain.Repository;

public interface ApplicantRepository
{
    Task CreateApplicant(Applicant applicant);

    Task CreateManager(Manager manager);
}