using StudentApp.Application.Models.Dto;

namespace StudentApp.Application.Abstraction;

/// <summary>
/// Provides methods for retrieving user information asynchronously.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Asynchronously deletes the user account associated with the specified email address using the provided user
    /// manager.
    /// </summary>
    /// <remarks>If no user with the specified email address exists, the method returns <see
    /// langword="false"/>. The operation is performed asynchronously and may be cancelled if the provided cancellation
    /// token is triggered.</remarks>
    /// <param name="mail">The email address of the user to delete. Cannot be null or empty.</param>
    /// <param name="userManager">The user manager instance used to perform the deletion operation. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user was
    /// successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> DeleteUserAsync(string mail, CancellationToken ct);

    /// <summary>
    /// Updates the specified user and applies any changes to their roles asynchronously.
    /// </summary>
    /// <remarks>The operation may update both user details and role assignments. Ensure that the provided
    /// user entity contains valid and up-to-date information. The method does not throw exceptions for update failures;
    /// instead, it returns null.</remarks>
    /// <param name="user">The user entity containing updated information to be applied.</param>
    /// <param name="roleManager">The role manager used to manage and update the user's roles.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a UserDto with the updated user
    /// information, or null if the update fails.</returns>
    Task<UserDto?> UpdateUserAsync(UserDto user, CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves all users from the data source.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of user data transfer
    /// objects. The array will be empty if no users are found.</returns>
    Task<UserDto[]> GetAllUsersAsync(CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves a user by their email address.
    /// </summary>
    /// <param name="mail">The email address of the user to retrieve. Cannot be null or empty.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="UserDto"/> representing
    /// the user with the specified email address, or <see langword="null"/> if no user is found.</returns>
    Task<UserDto?> GetUserByMailAsync(string mail, CancellationToken ct);
}