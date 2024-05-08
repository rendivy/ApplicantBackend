using Common.BaseModel;

namespace EnrollmentService.Domain.Entity;

public class Applicant : BaseEntity
{
    public required DateOnly DateOfBirth { get; set; }
    public required Gender Gender { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string FullName { get; set; }
    public required string Citizenship { get; set; }
}