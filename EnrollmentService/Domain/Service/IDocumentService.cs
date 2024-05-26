using EnrollmentService.Presentation.Model;

namespace EnrollmentService.Domain.Service;

public interface IDocumentService
{
    Task SaveDocumentScan(IFormFile file, string userId, string educationDocumentId);
    Task EditDocument(EducationDocumentRequest educationDocumentRequest, string educationDocumentId, string userId);
    Task AddPassportInformation(PassportDocumentRequest passportDocumentRequest, string userId);
    Task EditPassportInformation(PassportDocumentRequest passportDocumentRequest, string userId);
    Task<List<EducationDocumentResponse>> GetEducationDocumentInformation(string userId); 
    Task AddEducationDocumentInformation(EducationDocumentRequest educationDocumentRequest, string userId);
    Task SavePassport(IFormFile file);
}