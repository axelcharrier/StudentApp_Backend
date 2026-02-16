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

    public async Task<User> GetUserByMailAsync(string mail, CancellationToken ct)
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
}
