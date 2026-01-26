namespace StudentApp.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Models;
using System.Threading.Tasks;

/// <summary>
/// Represents an API controller that provides endpoints for managing student records, including retrieving, adding,
/// updating, and deleting students.
/// </summary>
[ApiController]
[Route("[controller]")]
public class StudentController(IStudentService studentService) : ControllerBase
{

    /// <summary>
    /// Retrieves all students as a collection of student data transfer objects.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="StudentDto"/> objects representing all students. The collection is empty
    /// if no students are found.</returns>
    [HttpGet]
    public async Task<IEnumerable<StudentDto>> GetAllStudent(CancellationToken ct)
    {
        var studentList = await studentService.GetAllStudents(ct);
        return studentList;
    }

    /// <summary>
    /// Retrieves the details of a student with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the student details if found; otherwise, a 404 Not Found result if no
    /// student exists with the specified identifier.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id, CancellationToken ct)
    {
        var student = await studentService.GetStudentById(id, ct);
        if (student is null)
            return NotFound(new { message = $"The student with this id doesnt exist" });
        return Ok(student);
    }

    /// <summary>
    /// Adds a new student to the system.
    /// </summary>
    /// <remarks>The request body must contain a valid student object.</remarks>
    /// <param name="student">The student information to add. Must not be null.</param>
    /// <returns>A 201 Created response with the added student if successful; otherwise, a 400 Bad Request response if the input
    /// is invalid or an error occurs.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddStudent([FromBody] StudentDto student, CancellationToken ct)
    {
        if (student is null)
            return BadRequest(new { message = "Student cannot be null" });
        try
        {
            var result = await studentService.AddStudent(student, ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates the details of an existing student using the provided student data.
    /// </summary>
    /// <param name="student">The student information to update. Must not be null.</param>
    /// <returns>An IActionResult that represents the result of the update operation. Returns 200 OK with the updated student if
    /// successful; otherwise, returns 400 Bad Request with an error message.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDto student, CancellationToken ct)
    {
        if (student is null)
            return BadRequest(new { message = "Student canno be null" });
        try
        {
            var updatedStudent = await studentService.UpdateStudent(id, student, ct);
            return Ok(updatedStudent);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    /// <summary>
    /// Deletes the student with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student to delete.</param>
    /// <returns>An <see cref="OkObjectResult"/> if the student was successfully deleted; otherwise, a <see
    /// cref="BadRequestObjectResult"/> if no student with the specified identifier exists.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id, CancellationToken ct)
    {
        if (await studentService.GetStudentById(id, ct) is null)
            return BadRequest("Cannot found student with this Id");

        return Ok(await studentService.DeleteStudent(id, ct));
    }
}
