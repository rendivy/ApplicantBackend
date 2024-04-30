using EnrollmentService.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentService.Data.Database;

public class EnrollmentDatabaseContext(DbContextOptions<EnrollmentDatabaseContext> options) : DbContext(options)
{
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>()
            .HasKey(e => e.Id);
        
        base.OnModelCreating(modelBuilder);
    }
}