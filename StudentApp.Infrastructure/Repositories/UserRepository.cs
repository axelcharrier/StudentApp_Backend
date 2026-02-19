namespace StudentApp.Infrastructure.Repositories;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Abstractions;
using StudentApp.Infrastructure.Persistence;

/// <inheritdoc/>
public class UserRepository(AppDbContext context) : IUserRepository
{
    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<IdentityUser?> GetIdentityUserAsync(string mail, CancellationToken ct, bool tracking = false)
    {
        IQueryable<IdentityUser> query = context.Users;
        query = tracking ? query : query.AsNoTracking();
        return await query.FirstOrDefaultAsync(u => u.UserName == mail, cancellationToken: ct);
    }

    /// <inheritdoc/>
    public async Task<IdentityUser?> UpdateUserAsync(IdentityUser userToUpdate, CancellationToken ct)
    {
        if (userToUpdate.UserName is null)
            return null;

        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return userToUpdate;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteUserAsync(string mail, CancellationToken ct)
    {
        var userToDelete = await GetIdentityUserAsync(mail, ct, true).ConfigureAwait(false);

        if (userToDelete is null) return false;

        context.Users.Remove(userToDelete);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> RoleExistsAsync(string roleName, CancellationToken ct) =>
        await context.Roles.AsAsyncEnumerable().AnyAsync(role => role.Name != null && role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase), cancellationToken: ct).ConfigureAwait(false);
}
