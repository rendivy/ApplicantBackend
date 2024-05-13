using HandbookService.Domain.Model.Education;

namespace HandbookService.Domain.Service;

public interface IHandbookService
{
    public Task ImportAllHandbookDataAsync();
    public Task<EducationProgram?> GetProgramByIdAsync(Guid id);
    public Task UpdateAllHandbookDataAsync();
}