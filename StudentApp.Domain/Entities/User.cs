namespace StudentApp.Domain.Entities;

public sealed record class User
{
    public required string Mail { get; set; }
    public bool IsMailConfirmed { get; set; }
    public required string Role { get; set; }
}
