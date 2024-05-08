using System.ComponentModel.DataAnnotations;

namespace EnrollmentService.Domain.Entity.Stuff;

public class Manager
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
}