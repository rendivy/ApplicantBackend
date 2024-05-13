using System.Reflection.Metadata;
using HandbookService.Domain.Model;
using HandbookService.Domain.Model.Education;
using Microsoft.EntityFrameworkCore;

namespace HandbookService.Infrastructure.Data;

public class HandbookDbContext(DbContextOptions<HandbookDbContext> options) : DbContext(options)
{
    public DbSet<EducationLevel> EducationLevel { get; set; }
    public DbSet<EducationDocumentType> DocumentType { get; set; }
    public DbSet<Faculty> Faculty { get; set; }
    public DbSet<EducationProgram?> Program { get; set; }
    public DbSet<Import> HandbookImport { get; set; }
    public DbSet<NextEducationLevel> NextEducationLevel { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dateTimeUtcConverter = new DateTimeUtcConverter();

        modelBuilder.Entity<EducationLevel>()
            .Property(el => el.Id)
            .ValueGeneratedNever();
        
        modelBuilder.Entity<NextEducationLevel>()
            .HasOne(nel => nel.EducationLevel)
            .WithMany()
            .HasForeignKey(nel => nel.EducationLevelId);
        
        modelBuilder.Entity<Faculty>().Property(
                e => e.CreateTime)
            .HasConversion(dateTimeUtcConverter);
        
        modelBuilder.Entity<EducationDocumentType>().Property(
                e => e.CreateTime)
            .HasConversion(dateTimeUtcConverter);
        
        modelBuilder.Entity<EducationProgram>().Property(
                e => e.CreateTime)
            .HasConversion(dateTimeUtcConverter);
        
        modelBuilder.Entity<EducationDocumentType>().Property(
                e => e.CreateTime)
            .HasConversion(dateTimeUtcConverter);

        base.OnModelCreating(modelBuilder);
    }
}