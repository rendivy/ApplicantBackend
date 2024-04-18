namespace HandbookService.Domain.Model.Education;

public class EducationDocumentType
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public int EducationLevelId { get; set; }
    public EducationLevel? EducationLevel { get; set; }
    public List<EducationLevel> NextEducationLevels { get; set; } = [];
}