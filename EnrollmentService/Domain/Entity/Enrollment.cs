namespace EnrollmentService.Domain.Entity;

public class Enrollment
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public Guid ProgramId { get; set; }
    public EnrollmentPriority EnrollmentPriority { get; set; }
    public EnrollmentStatus EnrollmentStatus { get; set; }
}