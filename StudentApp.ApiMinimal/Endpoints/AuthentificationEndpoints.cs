using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StudentApp.Application.Models.Payload;
using System.Text.RegularExpressions;

namespace StudentApp.ApiMinimal.Endpoints;

public static class AuthentificationEndpoints
{
    public static async Task Map(WebApplication application)
    {
        var authentificationRoute = application.MapGroup("/authentification");

        authentificationRoute.MapIdentityApi<IdentityUser>();

        authentificationRoute.MapPost("/createAccount", Register)
            .RequireAuthorization();

        authentificationRoute.MapPost("/logout", Logout)
            .RequireAuthorization();
    }

    private static async Task<IResult> Logout(SignInManager<IdentityUser> signInManager, [FromBody] object empty)
    {
        if (empty != null)
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }
        return Results.Unauthorized();
    }

    [Authorize(Roles = "Teacher")]
    private static async Task<IResult> Register(
        [FromBody] UserPayload registration,
        HttpContext context,
        [FromServices] RoleManager<IdentityRole> roleManager,
        [FromServices] UserManager<IdentityUser> userManager,
        [FromServices] IUserStore<IdentityUser> userStore)
    {
        string regexEmailPatern = """^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$""";

        var emailStore = (IUserEmailStore<IdentityUser>)userStore;
        var email = registration.Email;

        if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, regexEmailPatern))
        {
            return Results.BadRequest(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        }

        if (!await roleManager.RoleExistsAsync(registration.RoleName))
        {
            return Results.BadRequest("this role doesn't exists");
        }

        var user = new IdentityUser();

        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, registration.Password);
        var addingRole = await userManager.AddToRoleAsync(user, registration.RoleName);

        if (!result.Succeeded || !addingRole.Succeeded)
        {
            return Results.BadRequest(result);
        }

        return TypedResults.Ok();
    }
}
