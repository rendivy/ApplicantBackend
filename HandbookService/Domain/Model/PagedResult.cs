namespace HandbookService.Domain.Model;

public class PagedResult<T>
{
    public List<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}