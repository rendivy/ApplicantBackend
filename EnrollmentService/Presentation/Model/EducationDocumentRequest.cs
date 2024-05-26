using EnrollmentService.Domain.Entity.Document;

namespace EnrollmentService.Presentation.Model;

public class EducationDocumentRequest
{
    public required DateTime CreateTime { get; set; }
    public required string Name { get; set; } = string.Empty;
}

public class EducationDocumentResponse
{
    public required Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    public required string Name { get; set; } = string.Empty;
}

public class PassportDocumentRequest
{
    public required string SeriesAndNumber { get; set; }
    public required string PlaceOfBirth { get; set; }
    public required string IssuedBy { get; set; }
    public required DateTime DateOfIssue { get; set; }
}