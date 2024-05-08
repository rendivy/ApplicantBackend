using Common.BaseModel;

namespace EnrollmentService.Domain.Entity;

public class AdmissionProgram : BaseEntity
{
    public required DateTime CreateTime { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string? Code { get; set; }
    public required string Language { get; set; } = string.Empty;
    public required string EducationForm { get; set; } = string.Empty;
    public required Guid FacultyId { get; set; }
}