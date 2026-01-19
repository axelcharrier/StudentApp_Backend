namespace StudentApp.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using StudentApp.Domain.Entities;

/// <summary>
/// Represents the database context for the application
/// </summary>
/// <param name="options">The options to be used by the context. Cannot be null.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Student DbSet().
    /// </summary>
    public DbSet<Student> Students => this.Set<Student>();
}
