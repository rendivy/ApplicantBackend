using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HandbookService.Infrastructure.Data;

public class DateTimeUtcConverter() : ValueConverter<DateTime, DateTime>(
    v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));