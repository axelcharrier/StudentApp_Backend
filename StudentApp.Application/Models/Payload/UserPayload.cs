namespace StudentApp.Application.Models.Payload;

public sealed record UserPayload
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string RoleName { get; set; }
}
