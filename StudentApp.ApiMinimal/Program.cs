using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using StudentApp.ApiMinimal.Endpoints;
using StudentApp.ApiMinimal.Policies;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Extensions;
using StudentApp.Application.Implementations;
using StudentApp.Infrastructure.Abstractions;
using StudentApp.Infrastructure.Persistence;
using StudentApp.Infrastructure.Repositories;
using StudentApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add Identity services
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(UserPolicy.AllowTeacher,
        policy => policy.RequireRole(UserPolicy.TeacherRole, UserPolicy.AdminRole));
});

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();


// Cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAngularOrigins",
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:4200");
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowCredentials();
                      });
});

builder.Services.AddOpenApi();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = [UserPolicy.AdminRole, UserPolicy.TeacherRole, "Student"];

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    const string adminEmail = "admin@admin.fr";
    const string adminPassword = "Admin@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser is null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (!createAdminResult.Succeeded)
        {
            throw new InvalidOperationException(string.Join("; ", createAdminResult.Errors.Select(e => e.Description)));
        }
    }

    if (!await userManager.IsInRoleAsync(adminUser, UserPolicy.AdminRole))
    {
        var addToRoleResult = await userManager.AddToRoleAsync(adminUser, UserPolicy.AdminRole);
        if (!addToRoleResult.Succeeded)
        {
            throw new InvalidOperationException(string.Join("; ", addToRoleResult.Errors.Select(e => e.Description)));
        }
    }
}

#region Méthodes

await StudentsEndpoints.Map(app);
await AuthentificationEndpoints.Map(app);
await UsersEndpoints.Map(app);

#endregion 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowAngularOrigins");

if (app.Environment.IsProduction())
    app.UseHttpsRedirection();

await app.RunAsync();