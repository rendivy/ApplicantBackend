using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Entity.Document;
using EnrollmentService.Presentation.Model;

namespace EnrollmentService.Domain.Service;

public interface IManagerService
{
    public Task SetManagerOnEnrollment(string enrollmentId, string managerId);

    Task<PagedList<EnrollmentDTO>> GetApplicantEnrollment(string? name, string? program, Guid? faculties,
        EnrollmentStatus? status, bool unassigned, string managerId, int pageNumber, int pageSize, SortOrder sortOrder);

    public Task RemoveManagerFromEnrollment(string enrollmentId, string managerId);

    public Task<List<Document>> GetApplicantDocuments(string applicantId, string managerId);

    public Task<Applicant> GetApplicant(string applicantId, string managerId);

    public Task EditApplicantEducationDocument(string applicantId, string managerId, EducationDocument document);

    public Task SetApplicantEnrollmentStatus(string enrollmentId, EnrollmentStatus status, string message,
        string managerId);
}