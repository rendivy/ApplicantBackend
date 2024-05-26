namespace EnrollmentService.Domain.Service;

public interface IManagerService
{
    public Task SetManagerOnEnrollment(string enrollmentId, string managerId);
    
}