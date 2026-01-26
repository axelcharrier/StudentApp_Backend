namespace StudentApp.ApiMinimal.Endpoints;

using Microsoft.AspNetCore.Mvc;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Models;
using System.Threading.Tasks;


public static class StudentsEndpoints
{
    /// <summary>
    /// Configures the student-related API endpoints for the specified web application, including routes for retrieving,
    /// adding, updating, and deleting student records.
    /// </summary>
    /// <param name="application">The web application to which the student endpoints will be mapped. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation of mapping the student endpoints.</returns>
    public static async Task Map(WebApplication application)
    {
        var studentsRoute = application.MapGroup("/Students");


        studentsRoute.MapGet("", GetAllAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetAllStudents")
            .WithDisplayName("Get all students")
            .WithSummary("Get all students from de database");

        studentsRoute.MapGet("/GetById/{id}", GetByIdAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetStudentById")
            .WithDisplayName("Get a student by id")
            .WithSummary("Get a student thanks to his id");

        studentsRoute.MapPost("/Add", AddStudentAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("AddStudent")
            .WithDisplayName("Add a student")
            .WithSummary("Add a student to the database");

        studentsRoute.MapPut("/Update", UpdateStudentAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("UpdateStudent")
            .WithDisplayName("Update a student")
            .WithSummary("Update a student who already exists");

        studentsRoute.MapDelete("/Delete/{id}", DeleteStudentAsync)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteStudent")
            .WithDisplayName("Delete a student")
            .WithSummary("Permanently delete a student to the database");
    }

    #region Méthodes

    private static async Task<IResult> GetAllAsync(CancellationToken ct, [FromServices] IStudentService service)
    {
        var studentList = await service.GetAllStudents(ct);
        return Results.Ok(studentList);
    }

    private static async Task<IResult> GetByIdAsync(CancellationToken ct, int id, [FromServices] IStudentService service)
    {
        var student = await service.GetStudentById(id, ct);
        if (student is null)
            return Results.NotFound(new { message = $"The student with this id doesnt exist" });
        return Results.Ok(student);
    }

    private static async Task<IResult> UpdateStudentAsync(
                CancellationToken ct,
                [FromBody] StudentDto student,
                IStudentService service)
    {
        if (student is null)
            return Results.BadRequest(new { message = "Student canno be null" });
        try
        {
            var updatedStudent = await service.UpdateStudent(student, ct);
            return Results.Ok(updatedStudent);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }

    private static async Task<IResult> AddStudentAsync(
        CancellationToken ct,
        [FromBody] StudentDto student,
        [FromServices] IStudentService service)
    {
        if (service is null)
            return Results.BadRequest(new { message = "Student cannot be null" });
        try
        {
            var result = await service.AddStudent(student, ct);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }


    private static async Task<IResult> DeleteStudentAsync(
        CancellationToken ct,
        [FromRoute] int id,
        [FromServices] IStudentService service)
    {
        if (await service.GetStudentById(id, ct) is null)
            return Results.BadRequest("Cannot found student with this Id");

        return Results.Ok(await service.DeleteStudent(id, ct));



    }


    #endregion
}
