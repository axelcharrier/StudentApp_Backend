using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace StudentApp.ApiMinimal.Endpoints;

public static class AuthentificationEndpoints
{
    public static async Task Map(WebApplication application)
    {
        var authentificationRoute = application.MapGroup("/authentification");

        authentificationRoute.MapIdentityApi<IdentityUser>();

        authentificationRoute.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
            [FromBody] object empty) =>
                {
                    if (empty != null)
                    {
                        await signInManager.SignOutAsync();
                        return Results.Ok();
                    }
                    return Results.Unauthorized();
                })
        .RequireAuthorization();
    }
}
