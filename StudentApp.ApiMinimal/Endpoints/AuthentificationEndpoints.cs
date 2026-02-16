using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StudentApp.ApiMinimal.Models;
using StudentApp.ApiMinimal.Policies;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Models.Payload;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace StudentApp.ApiMinimal.Endpoints;

public static class AuthentificationEndpoints
{
    public static async Task Map(WebApplication application)
    {
        application.MapPost("/createAccount", Register)
            .RequireAuthorization(UserPolicy.AllowTeacher);

        application.MapPost("/login", Login);

        application.MapPost("/logout", Logout)
            .RequireAuthorization(UserPolicy.AllowTeacher);

        var manage = application.MapGroup("/manage/info");

        manage.MapGet("", UserInfos)
            .RequireAuthorization();

        manage.MapPost("", EditUserInfos)
            .RequireAuthorization(UserPolicy.AllowTeacher);

        application.MapGet("/getAllUsers", GetAllUsers)
            .RequireAuthorization(UserPolicy.AllowTeacher);
    }

    private static async Task<IResult> Login([FromBody] LoginRequest login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies, [FromServices] SignInManager<IdentityUser> signInManager)
    {
        var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
        var isPersistent = (useCookies == true) && (useSessionCookies != true);
        signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(login.TwoFactorCode))
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
            else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
        }

        if (!result.Succeeded)
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);

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
        string regexEmailPatern = """^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$""";

        var emailStore = (IUserEmailStore<IdentityUser>)userStore;
        var email = registration.Email;

        if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, regexEmailPatern))
            return Results.BadRequest(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));

        if (!await roleManager.RoleExistsAsync(registration.RoleName))
            return Results.BadRequest("this role doesn't exists");

        var user = new IdentityUser();

        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, registration.Password);
        var addingRole = await userManager.AddToRoleAsync(user, registration.RoleName);

        if (!result.Succeeded || !addingRole.Succeeded)
            return Results.BadRequest(result);

        return TypedResults.Ok();
    }

    private static async Task<IResult> UserInfos(ClaimsPrincipal claimsPrincipal, [FromServices] UserManager<IdentityUser> userManager)
    {
        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
            return TypedResults.NotFound();

        var email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email.");
        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        return Results.Ok(new ResponseInfo(email, isEmailConfirmed, roles[0]));
    }

    private static async Task<IResult> EditUserInfos(
        ClaimsPrincipal claimsPrincipal,
        [FromBody] InfoRequestCustom infoRequest,
        [FromServices] UserManager<IdentityUser> userManager)
    {
        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
            return TypedResults.NotFound();
        string regexEmailPatern = """^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$""";

        if (!string.IsNullOrEmpty(infoRequest.NewEmail) && !Regex.IsMatch(infoRequest.NewEmail, regexEmailPatern))
            return Results.BadRequest(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(infoRequest.NewEmail)));

        if (!string.IsNullOrEmpty(infoRequest.NewEmail))
        {
            user.Email = infoRequest.NewEmail;
            user.UserName = infoRequest.NewEmail;
            user.NormalizedUserName = infoRequest.NewEmail;
            user.NormalizedEmail = infoRequest.NewEmail;

            await userManager.UpdateAsync(user);
        }

        if (!string.IsNullOrEmpty(infoRequest.NewPassword))
        {
            if (string.IsNullOrEmpty(infoRequest.OldPassword))
                return Results.BadRequest("OldPasswordRequired");

            var changePasswordResult = await userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
            if (!changePasswordResult.Succeeded)
                return Results.BadRequest(changePasswordResult);
        }

        return TypedResults.Ok((user, userManager));
    }

    private static async Task<IResult> GetAllUsers(CancellationToken ct, [FromServices] IUserService userService)
    {
        var users = await userService.GetAllUsersAsync(ct);
        return Results.Ok(users);
    }
}
