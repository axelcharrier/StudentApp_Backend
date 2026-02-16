using StudentApp.Application.Models.Dto;

namespace StudentApp.Application.Abstraction;

/// <summary>
/// Provides methods for retrieving user information asynchronously.
/// </summary>
public interface IUserService
{
    Task<bool> DeleteUserAsync(string mail, Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser> userManager, CancellationToken ct);
    Task<UserDto?> UpdateUserAsync(Domain.Entities.User user, Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager, CancellationToken ct);
    Task<UserDto[]> GetAllUsersAsync(CancellationToken ct);
    Task<UserDto> GetUserByMailAsync(string mail, CancellationToken ct);
}