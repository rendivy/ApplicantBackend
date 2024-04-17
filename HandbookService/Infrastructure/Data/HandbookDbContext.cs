using Microsoft.EntityFrameworkCore;

namespace HandbookService.Infrastructure.Data;

public class HandbookDbContext(DbContextOptions<HandbookDbContext> options) : DbContext(options) {}


