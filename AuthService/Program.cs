using AuthService.Application.Services;
using AuthService.Configuration;
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

AuthConfiguration.AddJwt(builder.Services, builder.Configuration);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AuthDbContext>();

ServiceConfiguration.AddServices(builder.Services);
builder.Services.AddScoped<JwtProvider>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

//дефолтные роли

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();
}

app.Run();