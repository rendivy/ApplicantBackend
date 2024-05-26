using Common.Exception;
using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Entity.Document;
using EnrollmentService.Domain.Service;
using EnrollmentService.Presentation.Model;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentService.Presentation.Service;

public class DocumentService(EnrollmentDatabaseContext databaseContext) : IDocumentService
{
    public async Task SaveDocumentScan(IFormFile file, string userId, string educationDocumentId)
    {
        var educationDocument = await databaseContext.Document
            .FirstOrDefaultAsync(it => it.Id == new Guid(educationDocumentId));
        if (educationDocument == null)
        {
            throw new EnrollmentNotFound("Education document not found");
        }

        if (educationDocument.ApplicantId.ToString() != userId)
        {
            throw new UnauthorizedAccessException("You are not allowed to upload this document");
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, file.FileName);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
        educationDocument.FilePath = filePath;
        databaseContext.Update(educationDocument);
        await databaseContext.SaveChangesAsync();
    }

    public Task EditDocument(EducationDocumentRequest educationDocumentRequest, string educationDocumentId,
        string userId)
    {
        var educationDocument = databaseContext.EducationDocument
            .FirstOrDefault(it => it.Id == new Guid(educationDocumentId));
        if (educationDocument == null)
        {
            throw new EnrollmentNotFound("Education document not found");
        }

        if (educationDocument.ApplicantId.ToString() != userId)
        {
            throw new UnauthorizedAccessException("You are not allowed to edit this document");
        }

        educationDocument.Name = educationDocumentRequest.Name;
        educationDocument.CreateTime = educationDocumentRequest.CreateTime;
        databaseContext.Update(educationDocument);
        return databaseContext.SaveChangesAsync();
    }

    public Task AddPassportInformation(PassportDocumentRequest passportDocumentRequest, string userId)
    {
        var applicant = databaseContext.Applicant.FirstOrDefault(it => it.Id == new Guid(userId));
        if (applicant == null)
        {
            throw new UserNotFoundException("Applicant not found");
        }

        var passport = new Passport
        {
            Type = DocumentType.Passport,
            Applicant = applicant,
            ApplicantId = applicant.Id,
            SeriesAndNumber = passportDocumentRequest.SeriesAndNumber,
            PlaceOfBirth = passportDocumentRequest.PlaceOfBirth,
            IssuedBy = passportDocumentRequest.IssuedBy,
            DateOfIssue = passportDocumentRequest.DateOfIssue
        };
        databaseContext.Passport.Add(passport);
        return databaseContext.SaveChangesAsync();
    }

    public Task EditPassportInformation(PassportDocumentRequest passportDocumentRequest, string userId)
    {
        var passport = databaseContext.Passport.FirstOrDefault(it => it.ApplicantId.ToString() == userId);
        if (passport == null)
        {
            throw new EnrollmentNotFound("Passport not found");
        }

        passport.SeriesAndNumber = passportDocumentRequest.SeriesAndNumber;
        passport.PlaceOfBirth = passportDocumentRequest.PlaceOfBirth;
        passport.IssuedBy = passportDocumentRequest.IssuedBy;
        passport.DateOfIssue = passportDocumentRequest.DateOfIssue;
        databaseContext.Update(passport);
        return databaseContext.SaveChangesAsync();
    }

    public Task<List<EducationDocumentResponse>> GetEducationDocumentInformation(
        string userId)
    {
        var educationDocuments = databaseContext.EducationDocument
            .Where(it => it.ApplicantId.ToString() == userId)
            .Select(it => new EducationDocumentResponse
            {
                Id = it.Id,
                CreateTime = it.CreateTime,
                FilePath = it.FilePath,
                Name = it.Name
            }).ToList();
        return Task.FromResult(educationDocuments);
    }


    public Task AddEducationDocumentInformation(EducationDocumentRequest educationDocumentRequest, string userId)
    {
        var applicant = databaseContext.Applicant.FirstOrDefault(it => it.Id == new Guid(userId));
        if (applicant == null)
        {
            throw new UserNotFoundException("Applicant not found");
        }

        var educationDocument = new EducationDocument
        {
            Type = DocumentType.EducationDocument,
            Applicant = applicant,
            ApplicantId = applicant.Id,
            CreateTime = educationDocumentRequest.CreateTime,
            Name = educationDocumentRequest.Name,
            FilePath = null
        };
        databaseContext.EducationDocument.Add(educationDocument);
        return databaseContext.SaveChangesAsync();
    }

    public async Task SavePassport(IFormFile file)
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktopPath, file.FileName);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }
}