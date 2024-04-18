using HandbookService.Infrastructure.Service;

namespace HandbookService.Domain.Model.Education;

public class NextEducationLevel
{
    public int Id { get; set; } 
    public int EducationLevelId { get; set; }
    public EducationLevel EducationLevel { get; set; }
}