using Microsoft.EntityFrameworkCore;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Persistence;

namespace StudentApp.Infrastructure.Repositories;

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
}
