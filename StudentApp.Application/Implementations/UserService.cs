namespace StudentApp.Application.Implementations;

using StudentApp.Application.Abstraction;
using StudentApp.Application.Models.Dto;
using StudentApp.Infrastructure.Repositories;

public sealed class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserDto[]> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await userRepository.GetAllUsersAsync(ct);
        return [.. users.Select(user => new UserDto(user.Mail, user.IsMailConfirmed, user.Role))];
    }
}
