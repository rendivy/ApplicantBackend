using Common.RabbitModel.User;
using EasyNetQ;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.UseCase;

namespace EnrollmentService.Application.BackgroundWorkers;

public class UserCreatedMessageConsumer(IBus bus, IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return bus.PubSub.SubscribeAsync<ApplicantResponse>("user_created", Consume, stoppingToken);
    }

    private async Task Consume(ApplicantResponse message)
    {
        var applicant = new Applicant
        {
            Id = message.Id,
            FullName = message.FullName,
            DateOfBirth = message.DateOfBirth,
            Gender = message.Gender,
            Citizenship = message.Citizenship,
            Email = message.Email,
            PhoneNumber = message.PhoneNumber
        };
        using var scope = serviceScopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<CreateApplicantUseCase>();
        await service.Execute(applicant);
    }
}