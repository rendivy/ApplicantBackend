using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Repository;

namespace EnrollmentService.Data.Repository;

public class ApplicantRepositoryImpl(EnrollmentDatabaseContext dbContext) : ApplicantRepository
{
    private readonly EnrollmentDatabaseContext _dbContext = dbContext;

    public async Task CreateApplicant(Applicant applicant)
    {
        await _dbContext.Applicant.AddAsync(applicant);
        await _dbContext.SaveChangesAsync();
    }
}