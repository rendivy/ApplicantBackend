using HandbookService.Infrastructure.Service;

namespace HandbookService.Domain.Model.Education;

public class EducationDocumentType : IEntity<Guid>
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public int EducationLevelId { get; set; }
    public List<int> NextEducationLevelsIds { get; set; } = [];
}