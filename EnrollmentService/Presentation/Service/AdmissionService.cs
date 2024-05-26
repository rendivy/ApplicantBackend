using Common.Exception;
using Common.RabbitModel.Hanbook;
using EasyNetQ;
using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Service;
using EnrollmentService.Presentation.Model;
using EnrollmentService.Presentation.Util;
using Microsoft.EntityFrameworkCore;


namespace EnrollmentService.Presentation.Service;

public class AdmissionService : IAdmissionService
{
    private readonly EnrollmentDatabaseContext _dbContext;
    private readonly HttpClient _httpClient;
    private readonly IBus _bus;

    public AdmissionService(EnrollmentDatabaseContext dbContext, HttpClient httpClient, IBus bus)
    {
        _dbContext = dbContext;
        _httpClient = httpClient;
        _bus = bus;
    }

    public async Task CreateAdmission(AdmissionRequest admission, Guid applicantId)
    {
        var applicant = _dbContext.Applicant.FirstOrDefault(a => a.Id == applicantId);
        if (applicant == null)
        {
            throw new UserNotFoundException($"Applicant with this id not found");
        }

        if (!AdmissionRequestValidator.HasUniquePriorities(admission))
        {
            throw new NonUniquePriorityException("Priority in admission is not unique");
        }


        var enrollmentExists = _dbContext.Enrollment.Any(e => e.ApplicantId == applicantId);
        if (enrollmentExists)
        {
            throw new EnrollmentException();
        }

        var enrollmentId = Guid.NewGuid();

        var enrollment = new Enrollment
        {
            Id = enrollmentId,
            ApplicantId = applicantId,
            Applicant = applicant,
            LastUpdate = DateTime.Now.ToUniversalTime(),
            EnrollmentStatus = EnrollmentStatus.Created,
            EnrollmentPrograms = new List<EnrollmentPrograms>()
        };
        var educationLevelId = -1;
        foreach (var program in admission.Programs)
        {
            var admissionProgram = _dbContext.Program.FirstOrDefault(ap => ap.Id == program.AdmissionProgramId);
            if (admissionProgram == null)
            {
                var response = await _bus.Rpc.RequestAsync<Guid, HandbookModelRequest?>(
                    program.AdmissionProgramId, x => x.WithQueueName("handbook_getprogrambyid"));
                if (response == null)
                {
                    throw new EnrollmentProgramNotFound("Enrollment with this id not found");
                }

                if (educationLevelId == -1)
                {
                    if (admissionProgram != null) educationLevelId = admissionProgram.EducationLevelId;
                }
                else if (admissionProgram != null && educationLevelId != admissionProgram.EducationLevelId)
                {
                    throw new Exception("All programs in the admission must have the same education level");
                }

                admissionProgram = new AdmissionProgram
                {
                    Id = response.Id,
                    CreateTime = response.CreateTime,
                    Name = response.Name,
                    Code = response.Code,
                    Language = response.Language,
                    EducationForm = response.EducationForm,
                    FacultyId = response.FacultyId,
                    EducationLevelId = response.EducationLevelId
                };
            }

            await _dbContext.EnrollmentPrograms.AddAsync(new EnrollmentPrograms
            {
                EnrollmentId = enrollmentId,
                EnrollmentPriority = program.Priority,
                AdmissionProgram = admissionProgram
            });
        }

        await _dbContext.Enrollment.AddAsync(enrollment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EditAdmission(AdmissionRequest admission, Guid applicantId)
    {
        var applicant = _dbContext.Applicant.FirstOrDefault(a => a.Id == applicantId);
        if (applicant == null)
        {
            throw new UserNotFoundException($"Applicant with this id not found");
        }

        var enrollment = _dbContext.Enrollment
            .Include(e => e.EnrollmentPrograms)
            .FirstOrDefault(e => e.ApplicantId == applicantId);
        if (enrollment == null)
        {
            throw new EnrollmentNotFound("Enrollment for this applicant does not exist");
        }

        if (enrollment.EnrollmentStatus != EnrollmentStatus.Created &&
            enrollment.EnrollmentStatus != EnrollmentStatus.Rejected)
        {
            throw new EnrollmentProgramStatusException("Enrollment status is not 'Created' or 'Rejected'");
        }

        if (!AdmissionRequestValidator.HasUniquePriorities(admission))
        {
            throw new NonUniquePriorityException("Priority in admission is not unique");
        }

        enrollment.EnrollmentPrograms.Clear();
        var educationLevelId = -1;
        foreach (var program in admission.Programs)
        {
            var admissionProgram = _dbContext.Program.FirstOrDefault(ap => ap.Id == program.AdmissionProgramId);
            if (admissionProgram == null)
            {
                var response = await _bus.Rpc.RequestAsync<Guid, HandbookModelRequest?>(
                    program.AdmissionProgramId, x => x.WithQueueName("handbook_getprogrambyid"));
                if (response == null)
                {
                    throw new EnrollmentProgramNotFound("Enrollment with this id not found");
                }

                if (educationLevelId == -1)
                {
                    if (admissionProgram != null) educationLevelId = admissionProgram.EducationLevelId;
                }
                else if (admissionProgram != null && educationLevelId != admissionProgram.EducationLevelId)
                {
                    throw new Exception("All programs in the admission must have the same education level");
                }

                admissionProgram = new AdmissionProgram
                {
                    Id = response.Id,
                    CreateTime = response.CreateTime,
                    Name = response.Name,
                    Code = response.Code,
                    Language = response.Language,
                    EducationForm = response.EducationForm,
                    FacultyId = response.FacultyId,
                    EducationLevelId = response.EducationLevelId
                };
            }

            await _dbContext.EnrollmentPrograms.AddAsync(new EnrollmentPrograms
            {
                EnrollmentId = enrollment.Id,
                EnrollmentPriority = program.Priority,
                AdmissionProgram = admissionProgram
            });
        }

        enrollment.LastUpdate = DateTime.Now.ToUniversalTime();
        await _dbContext.SaveChangesAsync();
    }
}