using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentApp.ApiMinimal.Policies;
using StudentApp.Application.Abstraction;
using StudentApp.Domain.Entities;

namespace StudentApp.ApiMinimal.Endpoints;

public static class UsersEndpoints
{
    public static async Task Map(WebApplication application)
    {
        application.MapGet(string.Empty, GetAllUsersAsync)
            .RequireAuthorization(UserPolicy.AllowTeacher);

        application.MapGet("user", GetUserByMailAsync)
            .RequireAuthorization(UserPolicy.AllowTeacher);

        application.MapPut(string.Empty, UpdateUserAsync)
            .RequireAuthorization(UserPolicy.AllowTeacher);
    }
    private static async Task<IResult> GetAllUsersAsync(CancellationToken ct, [FromServices] IUserService userService)
    {
        var users = await userService.GetAllUsersAsync(ct);
        return Results.Ok(users);
    }

    private static async Task<IResult> GetUserByMailAsync([FromQuery] string mail, CancellationToken ct, [FromServices] IUserService userService)
    {
        var user = await userService.GetUserByMailAsync(mail, ct);
        if (user is null)
            return Results.NotFound(mail);
        return Results.Ok(user);
    }

    private static async Task<IResult> UpdateUserAsync([FromQuery] string mail, [FromBody] User user, [FromServices] IUserService userService, [FromServices] RoleManager<IdentityRole> roleManager, CancellationToken ct)
    {
        if (userService.GetUserByMailAsync(mail, ct) is null)
            return Results.NotFound(mail);

        var userUpdated = userService.UpdateUserAsync(user, roleManager, ct);

        return Results.Ok(userUpdated);
    }
}
