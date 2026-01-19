using Microsoft.EntityFrameworkCore;
using StudentApp.Domain.Models;

namespace StudentApp.Infrastructure.Persistence;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }
}
