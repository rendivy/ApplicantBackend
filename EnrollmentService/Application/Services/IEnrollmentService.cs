using EnrollmentService.Application.Model;

namespace EnrollmentService.Application.Services;

public interface IEnrollmentService
{
    public Task EnrollStudent(EnrollmentRequest request);
}