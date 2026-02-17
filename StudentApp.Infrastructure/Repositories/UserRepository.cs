using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Abstractions;
using StudentApp.Infrastructure.Persistence;

namespace StudentApp.Infrastructure.Repositories;

/// <summary>
/// Provides methods for retrieving user information from the application's data store.
/// </summary>
/// <remarks>This repository encapsulates data access logic for user-related operations. It is intended to be used
/// as a dependency in services that require user data. The class is not thread-safe; each instance should be used
/// within a single logical operation or request.</remarks>
/// <param name="context">The database context used to access user and role data. Cannot be null.</param>
public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User[]> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await context.Users.Select(user => new User
        {
            Mail = user.UserName!,
            IsMailConfirmed = user.EmailConfirmed,
            Role = context.Roles.FirstOrDefault(role =>
                role.Id == context.UserRoles.FirstOrDefault(userRole =>
                    userRole.UserId == user.Id)!.RoleId)!
                .Name!
        }).ToArrayAsync(cancellationToken: ct);

        return users;
    }

    public async Task<User?> GetUserByMailAsync(string mail, CancellationToken ct)
    {
        var user = await context.Users.Where(user => user.UserName == mail).Select(user => new User
        {
            Mail = user.UserName!,
            IsMailConfirmed = user.EmailConfirmed,
            Role = context.Roles.FirstOrDefault(role =>
                role.Id == context.UserRoles.FirstOrDefault(userRole =>
                    userRole.UserId == user.Id)!.RoleId)!
                .Name!
        }).FirstOrDefaultAsync(cancellationToken: ct);

        return user;
    }

    public async Task<User?> UpdateUserAsync(User user, RoleManager<IdentityRole> roleManager, CancellationToken ct)
    {
        var userToUpdate = await context.Users.Where(u => u.UserName == user.Mail).FirstOrDefaultAsync(ct);
        var userRoleToUpdate = await context.UserRoles.Where(ur => ur.UserId == userToUpdate!.Id).FirstOrDefaultAsync(ct);

        userToUpdate?.UserName = user.Mail;
        userToUpdate?.NormalizedUserName = user.Mail.ToUpper();
        userToUpdate?.Email = user.Mail;
        userToUpdate?.NormalizedEmail = user.Mail.ToUpper();

        userRoleToUpdate!.RoleId = await roleManager.GetRoleIdAsync(await context.Roles.Where(role => role.Name == user.Role).FirstOrDefaultAsync(ct));

        context.Users.Update(userToUpdate!);
        context.UserRoles.Update(userRoleToUpdate);

        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        var userResponse = await context.Users.Where(u => u.UserName == user.Mail).Select(user => new User
        {
            Mail = user.UserName!,
            IsMailConfirmed = user.EmailConfirmed,
            Role = context.Roles.FirstOrDefault(role =>
                role.Id == context.UserRoles.FirstOrDefault(userRole =>
                    userRole.UserId == user.Id)!.RoleId)!
                .Name!
        }).FirstOrDefaultAsync(cancellationToken: ct);

        return userResponse ?? null;
    }

    public async Task<bool> DeleteUserAsync(string mail, UserManager<IdentityUser> userManager, CancellationToken ct)
    {
        if (await context.Users.Where(user => user.UserName == mail).FirstOrDefaultAsync(ct) is not null)
        {
            await userManager.DeleteAsync(new IdentityUser(mail));
            await context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }
        else
        {
            return false;
        }
    }
}
