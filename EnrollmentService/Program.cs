using EnrollmentService.Application.Configuration;
using EnrollmentService.Data.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DatabaseConfiguration.AddDatabaseContext(builder.Services, builder.Configuration);


var app = builder.Build();

DatabaseConfiguration.AddAutoMigrations(app);
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


app.MapControllers();
app.UseHttpsRedirection();
app.Run();