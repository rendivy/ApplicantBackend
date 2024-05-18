using Common.BaseModel;
using EnrollmentService.Domain.Entity.Stuff;

namespace EnrollmentService.Domain.Entity;

public class Enrollment : BaseEntity
{
    public required Guid ApplicantId { get; set; }
    public required Applicant Applicant { get; set; }
    public required List<EnrollmentPrograms> EnrollmentPrograms { get; set; }
    public required DateTime LastUpdate { get; set; }
    public Guid? ManagerId { get; set; }
    public Manager? Manager { get; set; }
}
