using EasyNetQ;
using NotificationService.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IBus>(RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("EasyNetQ")));
builder.Services.AddHostedService<NotificationBackgroundService>();
var app = builder.Build();
app.UseHttpsRedirection();

app.Run();
