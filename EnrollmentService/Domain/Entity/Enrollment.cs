using AuthService.Domain.Entity;
using HandbookService.Domain.Model.Education;

namespace EnrollmentService.Domain.Entity;

//клонировать абитуриента из AuthService 
//запрос программы из HandbookService
public class Enrollment
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public User Applicant { get; set; }
    public Guid ProgramId { get; set; }
    public EducationProgram Program { get; set; }
    public EnrollmentPriority EnrollmentPriority { get; set; }
    public EnrollmentStatus EnrollmentStatus { get; set; }
}