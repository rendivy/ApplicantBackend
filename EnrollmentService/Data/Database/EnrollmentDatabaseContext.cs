using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Entity.Document;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentService.Data.Database;

public class EnrollmentDatabaseContext(DbContextOptions<EnrollmentDatabaseContext> options) : DbContext(options)
{
    public DbSet<Enrollment> Enrollment { get; set; }
    public DbSet<EnrollmentProgram> EnrollmentProgram { get; set; }
    public DbSet<Document> Document { get; set; }
    public DbSet<Passport> Passport { get; set; }
    public DbSet<EducationDocument> EducationDocument { get; set; }
    public DbSet<Applicant> Applicant { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>()
            .HasDiscriminator<string>("DocumentType")
            .HasValue<Passport>("passport")
            .HasValue<EducationDocument>("educationDocument");
    }
}