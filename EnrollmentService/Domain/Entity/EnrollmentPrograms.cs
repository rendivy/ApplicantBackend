using Common.BaseModel;

namespace EnrollmentService.Domain.Entity;

public class EnrollmentPrograms : BaseEntity
{
    public required Guid EnrollmentId { get; set; }
    public required AdmissionProgram? AdmissionProgram { get; set; }
    public required EnrollmentPriority EnrollmentPriority { get; set; }
}