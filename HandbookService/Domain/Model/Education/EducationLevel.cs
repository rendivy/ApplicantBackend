using HandbookService.Infrastructure.Service;

namespace HandbookService.Domain.Model.Education;

public class EducationLevel : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}