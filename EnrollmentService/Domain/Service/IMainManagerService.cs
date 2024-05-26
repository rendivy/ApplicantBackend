using EnrollmentService.Domain.Entity.Stuff;

namespace EnrollmentService.Domain.Service;

public interface IMainManagerService 
{
    public Task<List<Manager>> GetManagers (string userId, string role);
    public Task SetManagerOnEnrollment(string userId, string role, string managerId, string enrollmentId);
}