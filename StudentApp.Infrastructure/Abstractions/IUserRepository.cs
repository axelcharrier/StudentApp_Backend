using StudentApp.Domain.Entities;

namespace StudentApp.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User[]> GetAllUsersAsync(CancellationToken ct);
    }
}