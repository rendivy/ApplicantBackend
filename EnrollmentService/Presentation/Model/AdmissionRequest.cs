using EnrollmentService.Domain.Entity;

namespace EnrollmentService.Presentation.Model;

public record AdmissionRequest
{
    public required HashSet<EnrollmentProgramRequest> Programs { get; set; }
}

public record EnrollmentProgramRequest
{
    public required Guid AdmissionProgramId { get; set; }
    public required EnrollmentPriority Priority { get; set; }
}