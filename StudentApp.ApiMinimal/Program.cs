using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using StudentApp.ApiMinimal.Endpoints;
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

// Add Identity services
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>();

// Cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAngularOrigins",
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

builder.Services.AddOpenApi();


var app = builder.Build();

#region Méthodes

await StudentsEndpoints.Map(app);
app.MapIdentityApi<IdentityUser>();

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