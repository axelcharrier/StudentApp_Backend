namespace StudentApp.Infrastructure.Abstractions;

using Microsoft.AspNetCore.Identity;
using StudentApp.Domain.Entities;

/// <summary>
/// Represents a repository that provides asynchronous access to user data.
/// </summary>
/// <remarks>Implementations of this interface are responsible for retrieving user information from a data source.
/// Methods are asynchronous and support cancellation via a <see cref="CancellationToken"/>.</remarks>
public interface IUserRepository
{

    /// <summary>
    /// Deletes the user account associated with the specified email address asynchronously.
    /// </summary>
    /// <remarks>If no user exists with the specified email address, the method returns <see
    /// langword="false"/>. The operation is performed asynchronously and may be cancelled if the provided cancellation
    /// token is triggered.</remarks>
    /// <param name="mail">The email address of the user to delete. Cannot be null or empty.</param>
    /// <param name="userManager">The user manager instance used to perform user operations. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user was
    /// successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteUserAsync(string mail, CancellationToken ct);

    /// <summary>
    /// Updates the specified user's information asynchronously, including roles, and returns the updated user if the
    /// operation succeeds.
    /// </summary>
    /// <remarks>If the user does not exist or the update fails due to validation or role assignment issues,
    /// the method returns null. The operation is performed asynchronously and may be cancelled via the provided
    /// cancellation token.</remarks>
    /// <param name="user">The user object containing updated information to be applied. Cannot be null.</param>
    /// <param name="roleManager">The role manager used to validate and update the user's roles. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user if the update
    /// succeeds; otherwise, null.</returns>
    Task<IdentityUser?> UpdateUserAsync(IdentityUser userToUpdate, CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves all users from the data source.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of users. The array will be
    /// empty if no users are found.</returns>
    Task<User[]> GetAllUsersAsync(CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves the user associated with the specified email address.
    /// </summary>
    /// <param name="mail">The email address of the user to retrieve. Cannot be null or empty.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user associated with the
    /// specified email address, or null if no user is found.</returns>
    Task<User?> GetUserByMailAsync(string mail, CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves the identity user associated with the specified email address.
    /// </summary>
    /// <param name="mail">The email address of the user to retrieve. Cannot be null or empty.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <param name="tracking">Specifies whether the returned user entity should be tracked by the underlying data context. Set to <see
    /// langword="true"/> to enable tracking; otherwise, <see langword="false"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="IdentityUser"/>
    /// associated with the specified email address, or <see langword="null"/> if no user is found.</returns>
    Task<IdentityUser?> GetIdentityUserAsync(string mail, CancellationToken ct, bool tracking = false);
}