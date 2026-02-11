using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StudentApp.Application.Models.Payload;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace StudentApp.ApiMinimal.Endpoints;

public static class AuthentificationEndpoints
{
    public static async Task Map(WebApplication application)
    {
        var authentificationRoute = application.MapGroup("");

        //authentificationRoute.MapIdentityApi<IdentityUser>();

        authentificationRoute.MapPost("/createAccount", Register)
            .RequireAuthorization();

        authentificationRoute.MapPost("/login", Login);

        authentificationRoute.MapPost("/logout", Logout)
            .RequireAuthorization();

        authentificationRoute.MapGet("/manage/info", UserInfos)
            .RequireAuthorization();
    }

    private static async Task<IResult> Login([FromBody] LoginRequest login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies, [FromServices] IServiceProvider sp)
    {
        var signInManager = sp.GetRequiredService<SignInManager<IdentityUser>>();

        var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
        var isPersistent = (useCookies == true) && (useSessionCookies != true);
        signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(login.TwoFactorCode))
            {
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
            }
            else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
            {
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
            }
        }

        if (!result.Succeeded)
        {
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
        }

        // The signInManager already produced the needed response in the form of a cookie or bearer token.
        return TypedResults.Empty;
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

    private record ResponseInfo(string Email, Boolean IsMailConfirmed, string Role);
    private static async Task<IResult> UserInfos(ClaimsPrincipal claimsPrincipal, [FromServices] UserManager<IdentityUser> userManager, [FromServices] RoleManager<IdentityRole> roleManager)
    {
        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
        {
            return TypedResults.NotFound();
        }

        var email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email.");
        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        return Results.Ok(new ResponseInfo(email, isEmailConfirmed, roles[0]));
    }
}
