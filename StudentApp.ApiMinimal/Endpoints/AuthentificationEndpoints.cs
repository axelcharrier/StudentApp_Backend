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

        authentificationRoute.MapPost("/register", Register);

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

    private static async Task<IResult> Register([FromBody] UserPayload registration, HttpContext context, [FromServices] IServiceProvider sp)
    {
        string regexEmailPatern = """^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$""";

        var userManager = sp.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

        var userStore = sp.GetRequiredService<IUserStore<IdentityUser>>();
        var emailStore = (IUserEmailStore<IdentityUser>)userStore;
        var email = registration.Email;
        var roleName = registration.RoleName;

        if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, regexEmailPatern))
        {
            return Results.BadRequest(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        }

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            return Results.BadRequest("this role doesn't exists");
        }

        var user = new IdentityUser();

        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, registration.Password);
        var addingRole = await userManager.AddToRoleAsync(user, roleName);

        if (!result.Succeeded || !addingRole.Succeeded)
        {
            return Results.BadRequest(result);
        }

        return TypedResults.Ok();
    }
}
