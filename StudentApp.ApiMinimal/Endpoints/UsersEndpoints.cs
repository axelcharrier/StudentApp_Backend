using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentApp.ApiMinimal.Policies;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Models.Dto;

namespace StudentApp.ApiMinimal.Endpoints;

/// <summary>
/// Provides endpoint mappings for user management operations, including retrieving, updating, and deleting users.
/// Intended for integration with ASP.NET Core minimal APIs.
/// </summary>
/// <remarks>All endpoints require authorization with the teacher policy. The class defines routes for
/// user-related actions and should be used to register these endpoints within a web application during startup.
/// Endpoints are mapped for retrieving all users, retrieving a user by email, updating a user, and deleting a
/// user.</remarks>
public static class UsersEndpoints
{
    public static async Task Map(WebApplication application)
    {
        var userGroup = application.MapGroup("/users");

        userGroup.MapGet(string.Empty, GetAllUsersAsync)
            .RequireAuthorization(UserPolicy.AllowTeacher);

        userGroup.MapGet("bymail", GetUserByMailAsync)
            .RequireAuthorization(UserPolicy.AllowTeacher);

        userGroup.MapPut(string.Empty, UpdateUserAsync)
            .RequireAuthorization(UserPolicy.AllowTeacher);

        userGroup.MapDelete(string.Empty, DeleteUserAsync)
            .RequireAuthorization(UserPolicy.AllowTeacher);
    }
    private static async Task<IResult> GetAllUsersAsync(CancellationToken ct, [FromServices] IUserService userService)
    {
        var users = await userService.GetAllUsersAsync(ct).ConfigureAwait(false);
        return Results.Ok(users);
    }

    private static async Task<IResult> GetUserByMailAsync([FromQuery] string mail, CancellationToken ct, [FromServices] IUserService userService)
    {
        var user = await userService.GetUserByMailAsync(mail, ct).ConfigureAwait(false);
        if (user is null)
            return Results.NotFound(mail);
        return Results.Ok(user);
    }

    private static async Task<IResult> UpdateUserAsync([FromBody] UserDto user, [FromServices] IUserService userService, CancellationToken ct)
    {
        if (await userService.GetUserByMailAsync(user.Mail, ct).ConfigureAwait(false) is null)
            return Results.NotFound(user.Mail);

        var userUpdated = await userService.UpdateUserAsync(user, ct).ConfigureAwait(false);

        if (userUpdated is null)
            return Results.BadRequest(user.Mail);

        return Results.Ok(userUpdated);
    }

    private static async Task<IResult> DeleteUserAsync([FromQuery] string mail, [FromServices] IUserService userService, CancellationToken ct)
    {
        if (await userService.GetUserByMailAsync(mail, ct).ConfigureAwait(false) is null)
            return Results.NotFound(mail);

        if (!await userService.DeleteUserAsync(mail, ct).ConfigureAwait(false))
            return Results.BadRequest(mail);

        return Results.Ok(mail);
    }
}
