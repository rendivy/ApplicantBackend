using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Entity.Stuff;
using EnrollmentService.Domain.Service;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentService.Presentation.Service;

public class MainManagerService(EnrollmentDatabaseContext enrollmentDatabaseContext, IManagerService managerService)
    : IMainManagerService
{
    public async Task<List<Manager>> GetManagers(string userId, string role)
    {
        if (role != "MainManager")
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action");
        }

        var manager = await enrollmentDatabaseContext.Manager.FirstOrDefaultAsync(m => m.Id == new Guid(userId));
        if (manager == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action");
        }

        return await enrollmentDatabaseContext.Manager.ToListAsync();
    }

    public async Task SetManagerOnEnrollment(string userId, string role, string managerId, string enrollmentId)
    {
        if (role != "MainManager")
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action");
        }

        var manager = await enrollmentDatabaseContext.Manager.FirstOrDefaultAsync(m => m.Id == new Guid(userId));
        if (manager == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action");
        }

        await managerService.SetManagerOnEnrollment(enrollmentId, managerId);
    }
}