using Common.BaseModel;

namespace Common.RabbitModel.Hanbook;

public class HandbookModelRequest : BaseEntity
{
    public DateTime CreateTime { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string Language { get; set; } = string.Empty;
    public string EducationForm { get; set; } = string.Empty;
    public Guid FacultyId { get; set; }
    public int EducationLevelId { get; set; }
}