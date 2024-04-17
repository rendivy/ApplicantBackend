using EasyNetQ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IBus>(RabbitHutch.CreateBus("host=localhost;username=rmuser;password=rmpassword"));
builder.Services.AddHostedService<NotificationService.NotificationService>();
var app = builder.Build();
app.UseHttpsRedirection();

app.Run();