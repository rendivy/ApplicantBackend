using System.ComponentModel.DataAnnotations;

namespace EnrollmentService.Domain.Entity;

public class AdmissionProgram
{
    [Key] public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string Language { get; set; } = string.Empty;
    public string EducationForm { get; set; } = string.Empty;
    public Guid FacultyId { get; set; }
}