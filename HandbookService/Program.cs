using EasyNetQ;
using HandbookService.Domain.Service;
using HandbookService.Infrastructure.Data;
using HandbookService.Presentation.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<HandbookDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("HandbookDatabase"));
});
builder.Services.AddScoped<IHandbookService, HandbookService.Infrastructure.Service.HandbookService>();
builder.Services.AddSingleton(RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("EasyNetQ")));
builder.Services.QueueSubscribe(builder.Configuration);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HandbookDbContext>();
    await context.Database.MigrateAsync();
}

app.Run();