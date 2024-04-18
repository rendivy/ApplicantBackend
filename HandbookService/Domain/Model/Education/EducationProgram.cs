using HandbookService.Infrastructure.Service;

namespace HandbookService.Domain.Model.Education;

public class EducationProgram : IEntity<Guid>
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string Language { get; set; } = string.Empty;
    public string EducationForm { get; set; } = string.Empty;
    public Guid FacultyId { get; set; }
    public int EducationLevelId { get; set; }
}