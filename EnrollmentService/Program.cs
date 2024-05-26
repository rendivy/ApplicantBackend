using EasyNetQ;
using EnrollmentService.Application.BackgroundWorkers;
using EnrollmentService.Application.Configuration;
using EnrollmentService.Data.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date"
    });
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();
builder.Services.AddHttpClient();
DatabaseConfiguration.AddDatabaseContext(builder.Services, builder.Configuration);
builder.Services.ConfigureApplicantServices();
builder.Services.AddSingleton(RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("EasyNetQ")));
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


app.MapControllers();
app.UseHttpsRedirection();
app.Run();