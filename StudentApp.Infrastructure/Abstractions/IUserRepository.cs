namespace StudentApp.Infrastructure.Abstractions;

using StudentApp.Domain.Entities;

/// <summary>
/// Represents a repository that provides asynchronous access to user data.
/// </summary>
/// <remarks>Implementations of this interface are responsible for retrieving user information from a data source.
/// Methods are asynchronous and support cancellation via a <see cref="CancellationToken"/>.</remarks>
public interface IUserRepository
{
    Task<User[]> GetAllUsersAsync(CancellationToken ct);
}