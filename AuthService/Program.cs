using System.Text.Json.Serialization;
using AuthService.Application.Services;
using AuthService.Configuration;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Data.Database;
using Common.BaseModel;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


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
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSingleton<RedisDatabaseContext>(
    new RedisDatabaseContext(builder.Configuration.GetConnectionString("RedisDatabase")));
builder.Services.AddDbContext<AuthDbContext>(
    it => it.UseNpgsql(builder.Configuration.GetConnectionString("AuthDatabaseConnection")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();


AuthConfiguration.AddJwt(builder.Services, builder.Configuration);
ServiceConfiguration.AddServices(builder.Services);
builder.Services.AddSingleton<IBus>(RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("EasyNetQ")));

builder.Services.AddScoped<JwtProvider>();

var app = builder.Build();
MiddlewareConfiguration.AddMiddlewares(app);

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    await context.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach (var role in Enum.GetNames<Roles>())
    {
        if (!roleManager.RoleExistsAsync(role).Result)
        {
            roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var adminRole = Roles.Admin.ToString();
    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true,
            Citizenship = "Admin",
            DateOfBirth = new DateOnly(1990, 1, 1),
            FullName = "Admin",
            Gender = Gender.Male,
            PhoneNumber = "+7905553535"
        };
        userManager.Options.Password.RequireDigit = false;
        userManager.Options.Password.RequiredLength = 0;
        userManager.Options.Password.RequireNonAlphanumeric = false;
        userManager.Options.Password.RequireUppercase = false;
        userManager.Options.Password.RequireLowercase = false;
        var result = await userManager.CreateAsync(adminUser, "admin");
        userManager.Options.Password.RequireDigit = true;
        userManager.Options.Password.RequiredLength = 6;
        userManager.Options.Password.RequireNonAlphanumeric = true;
        userManager.Options.Password.RequireUppercase = true;
        userManager.Options.Password.RequireLowercase = true;
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();


app.Run();