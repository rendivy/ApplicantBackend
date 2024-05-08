using System.ComponentModel.DataAnnotations;
using Common.BaseModel;

namespace EnrollmentService.Domain.Entity.Stuff;

public class Manager : BaseEntity
{
    [MinLength(5)] 
    public required string FullName { get; set; }
    [EmailAddress] 
    public required string Email { get; set; }
}