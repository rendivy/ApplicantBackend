using System.Text.Json;
using Common.Exception;
using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Service;
using EnrollmentService.Presentation.Model;


namespace EnrollmentService.Presentation.Service;

public class AdmissionService : IAdmissionService
{
    private readonly EnrollmentDatabaseContext _dbContext;
    private readonly HttpClient _httpClient;

    public AdmissionService(EnrollmentDatabaseContext dbContext, HttpClient httpClient)
    {
        _dbContext = dbContext;
        _httpClient = httpClient;
    }

    public async Task CreateAdmission(AdmissionRequest admission, Guid applicantId)
    {
        var applicant = _dbContext.Applicant.FirstOrDefault(a => a.Id == applicantId);
        if (applicant == null)
        {
            throw new UserNotFoundException($"Applicant with this id not found");
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
                var response =
                    await _httpClient.GetAsync($"http://localhost:5178/api/handbook/{program.AdmissionProgramId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    admissionProgram = JsonSerializer.Deserialize<AdmissionProgram>(content);
                }
                else
                {
                    throw new Exception($"Admission program with id {program.AdmissionProgramId} not found");
                }
            }

            enrollment.EnrollmentPrograms.ToList().Add(new EnrollmentPrograms
            {
                EnrollmentId = enrollmentId,
                EnrollmentPriority = program.Priority,
                EnrollmentStatus = program.Status,
                AdmissionProgram = admissionProgram
            });
            _dbContext.Program.Update(admissionProgram);
        }

        _dbContext.Enrollment.Add(enrollment);
        await _dbContext.SaveChangesAsync();
    }
}