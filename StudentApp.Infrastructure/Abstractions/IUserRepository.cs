namespace StudentApp.Infrastructure.Abstractions;

using StudentApp.Domain.Entities;

/// <summary>
/// Represents a repository that provides asynchronous access to user data.
/// </summary>
/// <remarks>Implementations of this interface are responsible for retrieving user information from a data source.
/// Methods are asynchronous and support cancellation via a <see cref="CancellationToken"/>.</remarks>
public interface IUserRepository
{
    Task<bool> DeleteUserAsync(string mail, Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser> userManager, CancellationToken ct);
    Task<User?> UpdateUserAsync(User user, Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager, CancellationToken ct);
    Task<User[]> GetAllUsersAsync(CancellationToken ct);
    Task<User> GetUserByMailAsync(string mail, CancellationToken ct);
}