using Common.Exception;
using Common.RabbitModel.Email;
using EasyNetQ;
using EnrollmentService.Data.Database;
using EnrollmentService.Domain.Service;

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
        enrollmentDatabaseContext.Update(enrollment);
        await enrollmentDatabaseContext.SaveChangesAsync();
    }
}