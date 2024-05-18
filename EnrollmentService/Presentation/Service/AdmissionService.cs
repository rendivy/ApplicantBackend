using Common.Exception;
using Common.RabbitModel.Hanbook;
using EasyNetQ;
using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Service;
using EnrollmentService.Presentation.Model;
using Microsoft.EntityFrameworkCore;
using JsonSerializer = System.Text.Json.JsonSerializer;


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
            EnrollmentPrograms = new List<EnrollmentPrograms>()
        };

        foreach (var program in admission.Programs)
        {
            var admissionProgram = _dbContext.Program.FirstOrDefault(ap => ap.Id == program.AdmissionProgramId);
            if (admissionProgram == null)
            {
                var response = await _bus.Rpc.RequestAsync<Guid, HandbookModelRequest?>(
                    program.AdmissionProgramId, x => x.WithQueueName("handbook_getprogrambyid"));
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
            else
            {
                throw new Exception($"Admission program with id {program.AdmissionProgramId} not found");
            }

            await _dbContext.EnrollmentPrograms.AddAsync(new EnrollmentPrograms
            {
                EnrollmentId = enrollmentId,
                EnrollmentPriority = program.Priority,
                EnrollmentStatus = program.Status,
                AdmissionProgram = admissionProgram
            });
        }

        await _dbContext.Enrollment.AddAsync(enrollment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EditAdmission(AdmissionRequest admission, Guid applicantId)
    {
        var enrollment = _dbContext.Enrollment.Include(enrollment => enrollment.EnrollmentPrograms)
            .ThenInclude(enrollmentPrograms => enrollmentPrograms.AdmissionProgram)
            .FirstOrDefault(e => e.ApplicantId == applicantId);
        if (enrollment == null)
        {
            throw new EnrollmentNotFound("Enrollment for this applicant does not exist");
        }

        foreach (var program in admission.Programs)
        {
            var enrollmentProgram =
                enrollment.EnrollmentPrograms.FirstOrDefault(ep =>
                    ep.AdmissionProgram.Id == program.AdmissionProgramId);
            if (enrollmentProgram == null)
            {
                var response =
                    await _httpClient.GetAsync($"http://localhost:5178/api/handbook/{program.AdmissionProgramId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var admissionProgram = JsonSerializer.Deserialize<AdmissionProgram>(content);
                    enrollmentProgram = new EnrollmentPrograms
                    {
                        EnrollmentId = enrollment.Id,
                        EnrollmentPriority = program.Priority,
                        EnrollmentStatus = program.Status,
                        AdmissionProgram = admissionProgram
                    };
                    enrollment.EnrollmentPrograms.ToList().Add(enrollmentProgram);
                }
                else
                {
                    throw new Exception($"Admission program with id {program.AdmissionProgramId} not found");
                }
            }
            else
            {
                if (enrollmentProgram.EnrollmentStatus != EnrollmentStatus.Created &&
                    enrollmentProgram.EnrollmentStatus != EnrollmentStatus.Rejected)
                {
                    throw new EnrollmentProgramStatusException(
                        "Cannot change the program with status not equal to Created or Rejected");
                }

                enrollmentProgram.EnrollmentPriority = program.Priority;
                enrollmentProgram.EnrollmentStatus = program.Status;
            }
        }

        _dbContext.Enrollment.Update(enrollment);
        await _dbContext.SaveChangesAsync();
    }
}