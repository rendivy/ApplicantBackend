using Common.RabbitModel.Hanbook;
using EasyNetQ;
using HandbookService.Domain.Model.Education;
using HandbookService.Domain.Service;

namespace HandbookService.Presentation.Services;

public static class QueueListener
{
    public static void QueueSubscribe(this IServiceCollection services, IConfiguration configuration)
    {
        var bus = RabbitHutch.CreateBus(configuration.GetConnectionString("EasyNetQ"));
        var serviceProvider = services.BuildServiceProvider();
        var handbookService = serviceProvider.GetRequiredService<IHandbookService>();
        bus.Rpc.Respond<Guid, HandbookModelRequest?>(async request
                =>
            {
                var programBaseModel = await handbookService.GetProgramByIdAsync(request);
                return new HandbookModelRequest
                {
                    Id = programBaseModel.Id,
                    CreateTime = programBaseModel.CreateTime,
                    Name = programBaseModel.Name,
                    Code = programBaseModel.Code,
                    Language = programBaseModel.Language,
                    EducationForm = programBaseModel.EducationForm,
                    FacultyId = programBaseModel.FacultyId,
                    EducationLevelId = programBaseModel.EducationLevelId
                };
            },
            x => x.WithQueueName("handbook_getprogrambyid"));
    }
}