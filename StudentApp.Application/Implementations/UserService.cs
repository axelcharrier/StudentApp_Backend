namespace StudentApp.Application.Implementations;

using Microsoft.AspNetCore.Identity;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Models.Dto;
using StudentApp.Infrastructure.Abstractions;

/// <inheritdoc/>
public sealed class UserService(IUserRepository userRepository, UserManager<IdentityUser> userManager) : IUserService
{
    /// <inheritdoc/>
    public async Task<UserDto[]> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await userRepository.GetAllUsersAsync(ct).ConfigureAwait(false);
        return [.. users.Select(user => new UserDto(user.Mail, user.IsMailConfirmed, user.Role))];
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetUserByMailAsync(string mail, CancellationToken ct)
    {
        var user = await userRepository.GetUserByMailAsync(mail, ct).ConfigureAwait(false);
        if (user is null)
            return null;
        return new UserDto(user.Mail, user.IsMailConfirmed, user.Role);
    }

    /// <inheritdoc/>
    public async Task<UserDto?> UpdateUserAsync(UserDto user, CancellationToken ct)
    {
        var userToUpdate = await userRepository.GetIdentityUserAsync(user.Mail, ct, true).ConfigureAwait(false);
        if (userToUpdate is null) return null;

        var currentUserRole = (await GetUserByMailAsync(user.Mail, ct).ConfigureAwait(false))?.Role;

        if (currentUserRole != null && user.Role == "Student" && currentUserRole != user.Role && await IsLastTeacherAsync().ConfigureAwait(false)) return null;

        userToUpdate.Email = user.Mail;
        userToUpdate.NormalizedUserName = user.Mail.ToUpper();
        userToUpdate.UserName = user.Mail;
        userToUpdate.NormalizedUserName = user.Mail.ToUpper();
        userToUpdate.EmailConfirmed = user.IsMailConfirmed;

        var repositoryResponse = await userRepository.UpdateUserAsync(userToUpdate, ct).ConfigureAwait(false);
        if (repositoryResponse is null) return null;

        if (!await userRepository.RoleExistsAsync(user.Role, ct).ConfigureAwait(false))
            return null;

        var userIsInRole = (await userManager.GetUsersInRoleAsync(user.Role).ConfigureAwait(false)).FirstOrDefault(user => user.Id == userToUpdate.Id);

        if (userIsInRole is null)
        {
            var currentRole = (await userManager.GetRolesAsync(userToUpdate).ConfigureAwait(false))[0];
            if (currentRole is null) return null;

            await userManager.RemoveFromRoleAsync(userToUpdate, currentRole).ConfigureAwait(false);
            await userManager.AddToRoleAsync(userToUpdate, user.Role).ConfigureAwait(false);
        }

        return new UserDto(user.Mail, user.IsMailConfirmed, user.Role);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteUserAsync(string mail, CancellationToken ct)
    {
        var userRole = (await userRepository.GetUserByMailAsync(mail, ct).ConfigureAwait(false))?.Role;
        if (userRole != null && userRole.Equals("Teacher", StringComparison.OrdinalIgnoreCase) && await IsLastTeacherAsync().ConfigureAwait(false)) return false;

        var operationSuccess = await userRepository.DeleteUserAsync(mail, ct).ConfigureAwait(false);
        return operationSuccess;
    }

    public async Task<bool> IsLastTeacherAsync()
    {
        var users = await userManager.GetUsersInRoleAsync("Teacher").ConfigureAwait(false);
        return users.Count <= 1;
    }
}
