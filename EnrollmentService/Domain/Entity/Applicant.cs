using System.ComponentModel.DataAnnotations;
using AuthService.Domain.Entity;

namespace EnrollmentService.Domain.Entity;

public class Applicant
{
    [Key] public Guid Id { get; set; }
    [Required] public DateOnly DateOfBirth { get; set; }
    [Required] public Gender Gender { get; set; }
    [Required] public string FullName { get; set; }
    [Required] public string Citizenship { get; set; }
}