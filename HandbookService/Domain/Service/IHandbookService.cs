using HandbookService.Domain.Model;
using HandbookService.Domain.Model.Education;

namespace HandbookService.Domain.Service;

public interface IHandbookService
{
    public Task ImportAllHandbookDataAsync();
    public Task<EducationProgram?> GetProgramByIdAsync(Guid id);
    public Task UpdateAllHandbookDataAsync();

    Task<PagedResult<EducationProgram>> GetProgramsAsync(
        int pageNumber,
        int pageSize,
        Guid? facultyId = null,
        int? educationLevelId = null,
        string educationForm = null,
        string language = null,
        string searchTerm = null);
}