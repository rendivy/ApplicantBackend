using HandbookService.Infrastructure.Service;

namespace HandbookService.Domain.Model;

public class Faculty : IEntity<Guid>
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; }
}