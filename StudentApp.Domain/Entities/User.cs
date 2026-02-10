using Microsoft.AspNetCore.Identity;

namespace StudentApp.Domain.Entities;

public class AppUser : IdentityUser
{
    [PersonalData]
    public string? LastName { get; set; }

    [PersonalData]
    public string? FirstName { get; set; }

    public Boolean? IsTeacher { get; set; } = false;
}
