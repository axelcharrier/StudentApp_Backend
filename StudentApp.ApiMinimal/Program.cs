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
        policy => policy.RequireRole(UserPolicy.TeacherRole));
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