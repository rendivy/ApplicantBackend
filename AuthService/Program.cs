using System.Security.Claims;
using AuthService.Application.Services;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Data.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthorizationBuilder();

builder.Services.AddControllers();

builder.Services.AddDbContext<AuthDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDatabaseConnection")));


builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AuthDbContext>();


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
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
