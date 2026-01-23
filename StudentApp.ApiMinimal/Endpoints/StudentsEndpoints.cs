using Microsoft.AspNetCore.Mvc;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Models;


namespace StudentApp.ApiMinimal.Endpoints
{
    public static class StudentsEndpoints
    {
        public static async Task Map(WebApplication application)
        {
            var studentsRoute = application.MapGroup("/Students");

            studentsRoute.MapGet("/GetAll", async (CancellationToken ct, IStudentService service) =>
                await service.GetAllStudents(ct)
            )
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetAllStudents")
                .WithDisplayName("Get all students")
                .WithSummary("Get all students from de database");

            studentsRoute.MapGet("/GetById/{id}", async (CancellationToken ct, int id, IStudentService service) =>
                await service.GetStudentById(id, ct)
            )
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetStudentById")
                .WithDisplayName("Get a student by id")
                .WithSummary("Get a student thanks to his id"); ;

            studentsRoute.MapPost("/Add", async (
                    CancellationToken ct,
                    [FromBody] StudentDto student,
                    IStudentService service) =>
                await service.AddStudent(student, ct)
            )
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("AddStudent")
                .WithDisplayName("Add a student")
                .WithSummary("Add a student to the database"); ;

            studentsRoute.MapPut("/Update", async (
                    CancellationToken ct,
                    [FromBody] StudentDto student,
                    IStudentService service) =>
                await service.UpdateStudent(student, ct)
            )
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("UpdateStudent")
                .WithDisplayName("Update a student")
                .WithSummary("Update a student who already exists"); ;

            studentsRoute.MapDelete("/Delete/{id}", async (CancellationToken ct, int id, IStudentService service) =>
                await service.DeleteStudent(id, ct)
            )
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("DeleteStudent")
                .WithDisplayName("Delete a student")
                .WithSummary("Permanently delete a student to the database"); ;

        }
    }
}
