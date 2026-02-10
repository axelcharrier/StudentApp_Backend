namespace StudentApp.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentApp.Domain.Entities;

/// <summary>
/// Represents the database context for the application
/// </summary>
/// <param name="options">The options to be used by the context. Cannot be null.</param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    /// <summary>
    /// Student DbSet().
    /// </summary>
    public DbSet<Student> Students => this.Set<Student>();
    public DbSet<AppUser> AppUsers => this.Set<AppUser>();
}
