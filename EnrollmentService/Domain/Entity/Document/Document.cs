using Common.BaseModel;

namespace EnrollmentService.Domain.Entity.Document;

public class Document : BaseEntity
{
    public required DocumentType Type { get; set; }
    public required Guid ApplicantId { get; set; }
    public required Applicant Applicant { get; set; }
    public string? FilePath { get; set; }
}

public class Passport : Document
{
    public required string SeriesAndNumber { get; set; }
    public required string PlaceOfBirth { get; set; }
    public required string IssuedBy { get; set; }
    public required DateTime DateOfIssue { get; set; }
}

public class EducationDocument : Document
{
    public required DateTime CreateTime { get; set; }
    public required string Name { get; set; } = string.Empty;
}