namespace StudentApp.ApiMinimal.Models
{
    public sealed record InfoRequestCustom
    {
        public string? NewEmail { get; init; }
        public string? NewPassword { get; init; }
        public string? OldPassword { get; init; }
    }

    public sealed record ResponseInfo(string Email, bool IsMailConfirmed, string Role);

}
