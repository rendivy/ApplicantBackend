using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Repository;

namespace EnrollmentService.Data.Repository;

public class ApplicantRepositoryImpl(EnrollmentDatabaseContext dbContext) : ApplicantRepository
{
    public async Task CreateApplicant(Applicant applicant)
    {
        await dbContext.Applicant.AddAsync(applicant);
        await dbContext.SaveChangesAsync();
    }
}