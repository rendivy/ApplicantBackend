using EnrollmentService.Presentation.Model;

namespace EnrollmentService.Domain.Service;

public interface IAdmissionService
{
    public Task CreateAdmission(AdmissionRequest admission, Guid applicantId);
    public Task EditAdmission(AdmissionRequest admission, Guid applicantId);
}