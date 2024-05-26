using Common.Exception;
using Common.RabbitModel.Email;
using EasyNetQ;
using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Entity.Document;
using EnrollmentService.Domain.Service;
using EnrollmentService.Presentation.Model;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentService.Presentation.Service;

public class ManagerService(EnrollmentDatabaseContext enrollmentDatabaseContext, IBus bus) : IManagerService
{
    public async Task SetManagerOnEnrollment(string enrollmentId, string managerId)
    {
        var manager = enrollmentDatabaseContext.Manager.FirstOrDefault(it => it.Id == new Guid(managerId));
        if (manager == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to set manager on enrollment");
        }

        var enrollment = enrollmentDatabaseContext.Enrollment.FirstOrDefault(it => it.Id == new Guid(enrollmentId));
        if (enrollment == null)
        {
            throw new EnrollmentNotFound("Enrollment not found");
        }

        if (enrollment.ManagerId != null)
        {
            throw new ManagerAlreadySet("Manager already set on this enrollment");
        }

        var applicant = enrollmentDatabaseContext.Applicant.FirstOrDefault(it => it.Id == enrollment.ApplicantId);
        await bus.PubSub.PublishAsync(new EmailResponse
            {
                From = "admin@tsu.ru",
                To = applicant!.Email,
                Subject = "Registration",
                Message = "Manager has been set on your enrollment, please await for further instructions"
            }
        );
        enrollment.ManagerId = new Guid(managerId);
        enrollment.LastUpdate = DateTime.Now;
        enrollmentDatabaseContext.Update(enrollment);
        await enrollmentDatabaseContext.SaveChangesAsync();
    }


    public async Task<PagedList<EnrollmentDTO>> GetApplicantEnrollment(string? name, string? program, Guid? faculties,
        EnrollmentStatus? status, bool unassigned, string managerId, int pageNumber, int pageSize, SortOrder sortOrder)
    {
        var query = enrollmentDatabaseContext.Enrollment.Include(e => e.Applicant).Include(e => e.EnrollmentPrograms)
            .ThenInclude(ep => ep.AdmissionProgram).AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(e => e.Applicant.FullName.Contains(name));
        }

        if (!string.IsNullOrEmpty(program))
        {
            query = query.Where(e => e.EnrollmentPrograms.Any(ep => ep.AdmissionProgram!.Name == program));
        }

        if (faculties != null)
        {
            query = query.Where(e => e.EnrollmentPrograms.Any(ep =>
                ep.AdmissionProgram != null && (faculties == ep.AdmissionProgram.FacultyId)));
        }

        if (status.HasValue)
        {
            query = query.Where(e => e.EnrollmentStatus == status.Value);
        }

        if (unassigned)
        {
            query = query.Where(e => e.ManagerId == null);
        }

        if (!string.IsNullOrEmpty(managerId))
        {
            query = query.Where(e => e.ManagerId == new Guid(managerId));
        }

        query = sortOrder == SortOrder.Ascending
            ? query.OrderBy(e => e.LastUpdate)
            : query.OrderByDescending(e => e.LastUpdate);

        var dtoQuery = query.Select(e => new EnrollmentDTO
        {
            Id = e.Id,
            Applicant = e.Applicant,
            EnrollmentPrograms = e.EnrollmentPrograms,
            LastUpdate = e.LastUpdate,
            EnrollmentStatus = e.EnrollmentStatus,
            ManagerId = e.ManagerId,
            Manager = e.Manager
        });
        return await PagedList<EnrollmentDTO>.CreateAsync(dtoQuery, pageNumber, pageSize);
    }

    public async Task RemoveManagerFromEnrollment(string enrollmentId, string managerId)
    {
        var manager = enrollmentDatabaseContext.Manager.FirstOrDefault(it => it.Id == new Guid(managerId));
        var enrollment = enrollmentDatabaseContext.Enrollment.Include(enrollment => enrollment.Applicant)
            .FirstOrDefault(it => it.Id == new Guid(enrollmentId));
        if (manager == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to remove manager from enrollment");
        }

        if (enrollment == null)
        {
            throw new EnrollmentNotFound("Enrollment not found");
        }

        if (enrollment.ManagerId != new Guid(managerId))
        {
            throw new UnauthorizedAccessException("You are not authorized to remove manager from this enrollment");
        }

        enrollment.ManagerId = null;
        enrollment.LastUpdate = DateTime.Now;
        enrollmentDatabaseContext.Update(enrollment);
        await bus.PubSub.PublishAsync(new EmailResponse
            {
                From = "admin@tsu.ru",
                To = enrollment!.Applicant.Email,
                Subject = "Enrollment",
                Message = "Manager has been removed from your enrollment, please await for further instructions"
            }
        );
        await enrollmentDatabaseContext.SaveChangesAsync();
    }

    public Task<List<Document>> GetApplicantDocuments(string applicantId, string managerId)
    {
        var manager = enrollmentDatabaseContext.Manager.FirstOrDefault(it => it.Id == new Guid(managerId));
        if (manager == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to get applicant documents");
        }

        var documents = enrollmentDatabaseContext.Document
            .Where(it => it.ApplicantId == new Guid(applicantId)).ToList();
        return Task.FromResult(documents);
    }

    public Task<Applicant> GetApplicant(string applicantId, string managerId)
    {
        var manager = enrollmentDatabaseContext.Manager.FirstOrDefault(it => it.Id == new Guid(managerId));
        if (manager == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to get applicant documents");
        }

        var applicant = enrollmentDatabaseContext.Applicant.FirstOrDefault(it => it.Id == new Guid(applicantId));
        if (applicant == null)
        {
            throw new UserNotFoundException("Applicant not found");
        }

        return Task.FromResult(applicant);
    }

    public Task EditApplicantEducationDocument(string applicantId, string managerId, EducationDocument document)
    {
        var manager = enrollmentDatabaseContext.Manager.FirstOrDefault(it => it.Id == new Guid(managerId));

        if (manager == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to get applicant documents");
        }

        var educationDocument = enrollmentDatabaseContext.EducationDocument
            .FirstOrDefault(it => it.Id == document.Id);
        if (educationDocument == null)
        {
            throw new EnrollmentNotFound("Education document not found");
        }

        var enrollment = enrollmentDatabaseContext.Enrollment
            .Include(enrollment => enrollment.Applicant)
            .FirstOrDefault(it => it.ApplicantId == new Guid(applicantId));

        if (enrollment == null)
        {
            throw new EnrollmentNotFound("Enrollment not found");
        }

        if (enrollment.ManagerId != new Guid(managerId))
        {
            throw new UnauthorizedAccessException("You are not authorized to edit this document");
        }

        educationDocument.Name = document.Name;
        educationDocument.FilePath = document.FilePath;
        educationDocument.CreateTime = document.CreateTime;
        enrollmentDatabaseContext.Update(educationDocument);
        return enrollmentDatabaseContext.SaveChangesAsync();
    }


    public async Task SetApplicantEnrollmentStatus(string enrollmentId, EnrollmentStatus status, string message,
        string managerId)
    {
        var manager = enrollmentDatabaseContext.Manager.FirstOrDefault(it => it.Id == new Guid(managerId));
        if (manager == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to get applicant documents");
        }

        var enrollment = enrollmentDatabaseContext.Enrollment.Include(enrollment => enrollment.Applicant)
            .FirstOrDefault(it => it.Id == new Guid(enrollmentId));
        if (enrollment == null)
        {
            throw new EnrollmentNotFound("Enrollment not found");
        }

        await bus.PubSub.PublishAsync(new EmailResponse
            {
                From = "admin@tsu.ru",
                To = enrollment!.Applicant.Email,
                Subject = "Enrollment",
                Message = message
            }
        );
        enrollment.EnrollmentStatus = status;
        enrollment.LastUpdate = DateTime.Now;
        enrollmentDatabaseContext.Update(enrollment);
        await enrollmentDatabaseContext.SaveChangesAsync();
    }
}