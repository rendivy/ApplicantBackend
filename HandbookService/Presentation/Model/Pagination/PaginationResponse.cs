using HandbookService.Presentation.Model;

namespace HandbookService.Domain.Model.Pagination;

public class PaginationResponse
{
    public List<EducationProgramResponse>? Programs { get; set; }
    public PaginationPageInfoResponse Pagination { get; set; } = new();
}