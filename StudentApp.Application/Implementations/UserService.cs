namespace StudentApp.Application.Implementations;

using StudentApp.Application.Abstraction;
using StudentApp.Application.Models.Dto;
using StudentApp.Infrastructure.Abstractions;

/// <summary>
/// Provides user-related operations, such as retrieving user information, by interacting with the underlying user
/// repository.
/// </summary>
/// <param name="userRepository">The repository used to access and manage user data. Cannot be null.</param>
public sealed class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserDto[]> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await userRepository.GetAllUsersAsync(ct);
        return [.. users.Select(user => new UserDto(user.Mail, user.IsMailConfirmed, user.Role))];
    }
}
