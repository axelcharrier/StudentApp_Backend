namespace StudentApp.Application.Implementations;

using Microsoft.AspNetCore.Identity;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Models.Dto;
using StudentApp.Infrastructure.Abstractions;

/// <inheritdoc/>
public sealed class UserService(IUserRepository userRepository, UserManager<IdentityUser> userManager) : IUserService
{
    public async Task<UserDto[]> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await userRepository.GetAllUsersAsync(ct);
        return [.. users.Select(user => new UserDto(user.Mail, user.IsMailConfirmed, user.Role))];
    }

    public async Task<UserDto?> GetUserByMailAsync(string mail, CancellationToken ct)
    {
        var user = await userRepository.GetUserByMailAsync(mail, ct);
        if (user is null)
            return null;
        return new UserDto(user.Mail, user.IsMailConfirmed, user.Role);
    }

    public async Task<UserDto?> UpdateUserAsync(UserDto user, CancellationToken ct)
    {
        var userToUpdate = await userRepository.GetIdentityUserAsync(user.Mail, ct);

        if (userToUpdate is null)
            return null;

        userToUpdate.Email = user.Mail;
        userToUpdate.NormalizedUserName = user.Mail.ToUpper();
        userToUpdate.UserName = user.Mail;
        userToUpdate.NormalizedUserName = user.Mail.ToUpper();
        userToUpdate.EmailConfirmed = user.IsMailConfirmed;

        var repositoryResponse = await userRepository.UpdateUserAsync(userToUpdate, ct);
        if (repositoryResponse is null) return null;

        var userIsInRole = (await userManager.GetUsersInRoleAsync(user.Role)).FirstOrDefault(user => user.Id == userToUpdate.Id);

        if (userIsInRole is null)
        {
            var currentRole = (await userManager.GetRolesAsync(userToUpdate))[0];
            if (currentRole is null) return null;

            await userManager.RemoveFromRoleAsync(userToUpdate, currentRole);
            await userManager.AddToRoleAsync(userToUpdate, user.Role);
        }

        return new UserDto(user.Mail, user.IsMailConfirmed, user.Role);
    }

    public async Task<bool> DeleteUserAsync(string mail, CancellationToken ct)
    {
        var operationSuccess = await userRepository.DeleteUserAsync(mail, ct);
        return operationSuccess;
    }
}
