using HandbookService.Domain.Model;
using Newtonsoft.Json;

namespace HandbookService.Presentation.Model;

public class EducationDocumentTypeResponse
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    [JsonProperty("educationLevel")]
    public EducationLevelResponse? EducationLevelResponse { get; set; }
    public List<EducationLevelResponse> NextEducationLevels { get; set; } = [];
}

