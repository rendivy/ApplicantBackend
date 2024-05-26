using AuthService.Domain.Entity;
using Common.RabbitModel.User;
using EasyNetQ;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Entity.Stuff;
using EnrollmentService.Domain.UseCase;

namespace EnrollmentService.Application.BackgroundWorkers;

public class UserCreatedMessageConsumer(IBus bus, IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return bus.PubSub.SubscribeAsync<UserRabbitResponse>("user_created", Consume, stoppingToken);
    }

    private async Task Consume(UserRabbitResponse message)
    {
        using var scope = serviceScopeFactory.CreateScope();
        if (message.Roles == Roles.Applicant.ToString())
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
            var service = scope.ServiceProvider.GetRequiredService<CreateApplicantUseCase>();
            await service.Execute(applicant);
        }
        else
        {
            var manager = new Manager
            {
                Email = message.Email,
                FullName = message.FullName,
                Id = message.Id,
            };
            var service = scope.ServiceProvider.GetRequiredService<CreateManagerUseCase>();
            await service.Execute(manager);
        }
    }
}