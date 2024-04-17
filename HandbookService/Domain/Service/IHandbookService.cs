namespace HandbookService.Domain.Service;

public interface IHandbookService
{
    public Task ImportAllHandbookDataAsync();
    public Task UpdateAllHandbookDataAsync();
}