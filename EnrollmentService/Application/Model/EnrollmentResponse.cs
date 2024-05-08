using EnrollmentService.Domain.Entity;

namespace EnrollmentService.Application.Model;

public class EnrollmentRequest
{
    public List<EnrollmentProgramRequest> EnrollmentPrograms { get; set; }
}

public class EnrollmentProgramRequest
{
    public Guid AdmissionProgramId { get; set; }
    public EnrollmentPriority EnrollmentPriority { get; set; }
}