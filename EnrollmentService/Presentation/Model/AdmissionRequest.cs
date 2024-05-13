using EnrollmentService.Domain.Entity;
using EnrollmentService.Presentation.Util;

namespace EnrollmentService.Presentation.Model;

public record AdmissionRequest
{
    [UniqueElements] 
    [UniquePriorities] 
    public required HashSet<EnrollmentProgramRequest> Programs { get; set; }
}

public record EnrollmentProgramRequest
{
    public required Guid AdmissionProgramId { get; set; }
    public required EnrollmentPriority Priority { get; set; }
    public required EnrollmentStatus Status { get; set; } = EnrollmentStatus.Created;
}