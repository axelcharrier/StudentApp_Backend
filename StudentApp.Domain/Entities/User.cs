namespace StudentApp.Domain.Entities;

public sealed record class User
{
    public string Mail { get; set; }
    public bool IsMailConfirmed { get; set; }
    public string Role { get; set; }
}
