using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Entity.Stuff;
using EnrollmentService.Domain.Repository;

namespace EnrollmentService.Data.Repository;

public class ApplicantRepositoryImpl(EnrollmentDatabaseContext dbContext) : ApplicantRepository
{
    public async Task CreateApplicant(Applicant applicant)
    {
        await dbContext.Applicant.AddAsync(applicant);
        await dbContext.SaveChangesAsync();
    }

    public async Task CreateManager(Manager manager)
    {
        await dbContext.Manager.AddAsync(manager);
        await dbContext.SaveChangesAsync();
    }
}