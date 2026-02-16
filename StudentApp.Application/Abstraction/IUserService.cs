using StudentApp.Application.Models.Dto;

namespace StudentApp.Application.Abstraction
{
    public interface IUserService
    {
        Task<UserDto[]> GetAllUsersAsync(CancellationToken ct);
    }
}