using System.Net.Mail;
using Common.RabbitModel.Email;
using EasyNetQ;

namespace NotificationService.Application;

public class NotificationBackgroundService(IBus bus) : BackgroundService
{
    private const string HOST = "localhost";
    private const int PORT = 1025;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await bus.PubSub.SubscribeAsync<EmailResponse>(
            "notification", CreateNotificationBody, stoppingToken);
    }


    private async Task CreateNotificationBody(EmailResponse emailResponse)
    {
        using var smtpClient = new SmtpClient(HOST, PORT);
        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailResponse.From),
            Subject = emailResponse.Subject,
            Body = emailResponse.Message,
            IsBodyHtml = false
        };
        mailMessage.To.Add(emailResponse.To);
        await smtpClient.SendMailAsync(mailMessage);
    }
}