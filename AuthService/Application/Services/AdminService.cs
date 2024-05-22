using AuthService.Application.Utils;
using AuthService.Domain.Entity;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data.Database;
using AuthService.Presentation.Models.Admin;
using Common.RabbitModel.Email;
using Common.RabbitModel.Manager;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services;

public class AdminService(
    AuthDbContext authDbContext,
    UserManager<User> userManager,
    RedisDatabaseContext redisRepository,
    JwtProvider jwtProvider,
    IBus bus) : IAdminService
{
    public async Task CreateManager(CreateManagerRequest createManagerRequest, string userRole)
    {
        if (userRole != Roles.Admin.ToString())
        {
            throw new Exception("You don't have permission to create manager");
        }
        
        var user = new User
        {
            UserName = createManagerRequest.Email,
            Email = createManagerRequest.Email,
            FullName = createManagerRequest.FullName,
            PhoneNumber = createManagerRequest.PhoneNumber,
            Citizenship = createManagerRequest.Citizenship,
            DateOfBirth = createManagerRequest.DateOfBirth,
            Gender = createManagerRequest.Gender
        };

        var userPassword = PasswordGenerator.GeneratePassword(12);
        var result = await userManager.CreateAsync(user, userPassword);
        if (!result.Succeeded)
        {
            throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        await userManager.AddToRoleAsync(user, Roles.Manager.ToString());
        await bus.PubSub.PublishAsync(new EmailResponse
            {
                From = "admin@tsu.ru",
                To = user.Email,
                Subject = "Registration",
                Message =
                    $"You have been registered as a manager. " + Environment.NewLine +
                    $"Your credentials for login in system: {Environment.NewLine} Login: {createManagerRequest.Email} {Environment.NewLine} Password: {userPassword}"
            }
        );
    }

    public async Task CreateMainManager(CreateManagerRequest createManagerRequest, string userRole)
    {
        if (userRole != Roles.Admin.ToString())
        {
            throw new Exception("You don't have permission to create manager");
        }
        
        var user = new User
        {
            UserName = createManagerRequest.Email,
            Email = createManagerRequest.Email,
            FullName = createManagerRequest.FullName,
            PhoneNumber = createManagerRequest.PhoneNumber,
            Citizenship = createManagerRequest.Citizenship,
            DateOfBirth = createManagerRequest.DateOfBirth,
            Gender = createManagerRequest.Gender
        };

        var userPassword = PasswordGenerator.GeneratePassword(12);
        var result = await userManager.CreateAsync(user, userPassword);
        if (!result.Succeeded)
        {
            throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        await userManager.AddToRoleAsync(user, Roles.MainManager.ToString());
        await bus.PubSub.PublishAsync(new EmailResponse
            {
                From = "admin@tsu.ru",
                To = user.Email,
                Subject = "Registration",
                Message =
                    $"You have been registered as a manager. " + Environment.NewLine +
                    $"Your credentials for login in system: {Environment.NewLine} Login: {createManagerRequest.Email} {Environment.NewLine} Password: {userPassword}"
            }
        );
    }
}