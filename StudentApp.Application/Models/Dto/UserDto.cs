namespace StudentApp.Application.Models.Dto;

/// <summary>
/// Represents a data transfer object containing user account information, including email address, confirmation status,
/// and assigned role.
/// </summary>
/// <param name="Mail">The email address associated with the user account. Cannot be null or empty.</param>
/// <param name="IsMailConfirmed">A value indicating whether the user's email address has been confirmed. Set to <see langword="true"/> if the email
/// is confirmed; otherwise, <see langword="false"/>.</param>
/// <param name="Role">The role assigned to the user, such as 'Admin' or 'User'. Cannot be null or empty.</param>
public sealed record UserDto(string Mail, bool IsMailConfirmed, string Role) { }