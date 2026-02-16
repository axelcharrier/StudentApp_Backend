using StudentApp.Application.Models.Dto;

namespace StudentApp.Application.Abstraction;

/// <summary>
/// Provides methods for retrieving user information asynchronously.
/// </summary>
public interface IUserService
{
    Task<UserDto[]> GetAllUsersAsync(CancellationToken ct);
    Task<UserDto> GetUserByMailAsync(string mail, CancellationToken ct);
}