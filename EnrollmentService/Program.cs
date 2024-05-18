using EasyNetQ;
using EnrollmentService.Application.BackgroundWorkers;
using EnrollmentService.Application.Configuration;
using EnrollmentService.Data.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();
builder.Services.AddHttpClient();
DatabaseConfiguration.AddDatabaseContext(builder.Services, builder.Configuration);
builder.Services.ConfigureApplicantServices();
builder.Services.AddHostedService<UserCreatedMessageConsumer>();
JwtConfiguration.AddJwt(builder.Services, builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EnrollmentDatabaseContext>();
    await context.Database.MigrateAsync();
}

app.UseMiddleware<EnrollmentExceptionMiddleware>();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();