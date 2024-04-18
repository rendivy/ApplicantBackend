namespace HandbookService.Domain.Model.Pagination;

public class PaginationPageInfoResponse
{
    public int Size { get; set; }
    public int Count { get; set; }
    public int Current { get; set; }
}