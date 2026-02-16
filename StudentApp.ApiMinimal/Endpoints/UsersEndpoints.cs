using Microsoft.AspNetCore.Mvc;
using StudentApp.ApiMinimal.Policies;
using StudentApp.Application.Abstraction;

namespace StudentApp.ApiMinimal.Endpoints;

public static class UsersEndpoints
{
    public static async Task Map(WebApplication application)
    {
        application.MapGet(string.Empty, GetAllUsersAsync)
            .RequireAuthorization(UserPolicy.AllowTeacher);

        application.MapGet("user", GetUserByMailAsync)
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
}
