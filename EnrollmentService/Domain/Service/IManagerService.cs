using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Entity.Document;

namespace EnrollmentService.Domain.Service;

public interface IManagerService
{
    public Task SetManagerOnEnrollment(string enrollmentId, string managerId);

    public Task<List<Document>> GetApplicantDocuments(string applicantId, string managerId);

    public Task SetApplicantEnrollmentStatus(string enrollmentId, EnrollmentStatus status, string message,
        string managerId);
}