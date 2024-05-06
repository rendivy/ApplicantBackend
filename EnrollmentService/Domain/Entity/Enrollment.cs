using System.ComponentModel.DataAnnotations;
using HandbookService.Domain.Model.Education;

namespace EnrollmentService.Domain.Entity;

//клонировать абитуриента из AuthService 
//запрос программы из HandbookService
public class Enrollment
{
    [Key] public Guid Id { get; set; }
    [Required] public Guid ApplicantId { get; set; }
    [Required] public Applicant Applicant { get; set; }
    [Required] public Guid ProgramId { get; set; }
    [Required] public EnrollmentProgram EnrollmentProgram { get; set; }
    [Required] public EnrollmentPriority EnrollmentPriority { get; set; }
    [Required] public EnrollmentStatus EnrollmentStatus { get; set; }
}