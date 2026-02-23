namespace StudentApp.ApiMinimal.Models
{
    public sealed record ResponseInfo(string Email, bool IsMailConfirmed, string Role);
}
