using Scalar.AspNetCore;
using StudentApp.ApiMinimal.Endpoints;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Extensions;
using StudentApp.Application.Implementations;
using StudentApp.Infrastructure.Abstractions;
using StudentApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

// Add services to the container.
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

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

#endregion 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowAngularOrigins");

app.UseHttpsRedirection();

await app.RunAsync();