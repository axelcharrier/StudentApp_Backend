namespace StudentApp.Domain.Entities;

/// <summary>
/// Represents a user account with associated email, confirmation status, and role information.
/// </summary>
public sealed record class User
{
    public required string Mail { get; set; }
    public bool IsMailConfirmed { get; set; }
    public required string Role { get; set; }
}
