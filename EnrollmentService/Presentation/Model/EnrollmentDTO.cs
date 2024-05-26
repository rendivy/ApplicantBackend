using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Entity.Stuff;

namespace EnrollmentService.Presentation.Model;

public class EnrollmentDTO
{
    public Guid Id { get; set; }
    public Applicant Applicant { get; set; }
    public List<EnrollmentPrograms> EnrollmentPrograms { get; set; }
    public DateTime LastUpdate { get; set; }
    public EnrollmentStatus EnrollmentStatus { get; set; }
    public Guid? ManagerId { get; set; }
    public Manager? Manager { get; set; }
}