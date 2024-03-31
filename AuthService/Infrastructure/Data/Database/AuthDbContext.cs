using AuthService.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data.Database;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext<User>(options);

