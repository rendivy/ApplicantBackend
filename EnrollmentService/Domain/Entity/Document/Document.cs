using System.ComponentModel.DataAnnotations;

namespace EnrollmentService.Domain.Entity.Document;

public class Document
{
    [Key] public Guid Id { get; set; }
    [Required] public DocumentType Type { get; set; }
    [Required] public Guid ApplicantId { get; set; }
    [Required] public Applicant Applicant { get; set; }
}

public class Passport : Document
{
    [Required] public string SeriesAndNumber { get; set; }
    [Required] public string PlaceOfBirth { get; set; }
    [Required] public string IssuedBy { get; set; }
    [Required] public DateTime DateOfIssue { get; set; }
}

public class EducationDocument : Document
{
    [Required] public DateTime CreateTime { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
}