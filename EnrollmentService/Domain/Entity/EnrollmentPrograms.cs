using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.BaseModel;
using EnrollmentService.Domain.Entity.Stuff;

namespace EnrollmentService.Domain.Entity;

public class EnrollmentPrograms : BaseEntity
{
    public required Guid EnrollmentId { get; set; }
    public required Enrollment Enrollment { get; set; }
    public required AdmissionProgram AdmissionProgram { get; set; }
    public required EnrollmentPriority EnrollmentPriority { get; set; }
    public required EnrollmentStatus EnrollmentStatus { get; set; }
}