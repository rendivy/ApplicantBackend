namespace HandbookService.Domain.Model;

public class EducationDocumentTypeResponse
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public EducationLevelResponse EducationLevelResponse { get; set; } = new();
    public List<EducationLevelResponse> NextEducationLevels { get; set; } = [];
}

