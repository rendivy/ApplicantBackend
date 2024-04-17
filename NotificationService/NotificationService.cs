using System.Net.Mail;
using EasyNetQ;
using Messages;

namespace NotificationService;

public class NotificationService(IBus bus) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await bus.PubSub.SubscribeAsync<EmailNotification>("notification_subscription", HandleNotification,
            cancellationToken: stoppingToken);
    }

    private async Task HandleNotification(EmailNotification notification)
    {
        using var smtpClient = new SmtpClient("localhost", 1025);
        var mailMessage = new MailMessage
        {
            From = new MailAddress(notification.From),
            Subject = notification.Subject,
            Body = notification.Message,
            IsBodyHtml = false
        };
        mailMessage.To.Add(notification.To);
        await smtpClient.SendMailAsync(mailMessage);
    }
}