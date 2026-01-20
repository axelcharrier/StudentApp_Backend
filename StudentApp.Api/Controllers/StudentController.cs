namespace StudentApp.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using StudentApp.Application.Implementations;
using StudentApp.Application.Models;

/// <summary>
/// Represents an API controller that provides endpoints for managing student records, including retrieving, adding,
/// updating, and deleting students.
/// </summary>
[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService StudentService;

    public StudentController(IStudentService studentService)
    {
        StudentService = studentService;
    }

    /// <summary>
    /// Retrieves all students as a collection of student data transfer objects.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="StudentDto"/> objects representing all students. The collection is empty
    /// if no students are found.</returns>
    [HttpGet]
    public IEnumerable<StudentDto> GetAllStudent()
    {
        var StudentList = StudentService.GetAllStudents();
        return StudentList;
    }

    /// <summary>
    /// Retrieves the details of a student with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the student details if found; otherwise, a 404 Not Found result if no
    /// student exists with the specified identifier.</returns>
    [HttpGet("{id}")]
    public IActionResult GetStudentById(int id)
    {
        var student = StudentService.GetStudentById(id);
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
    public IActionResult AddStudent([FromBody] StudentDto student)
    {
        if (student is null)
            return BadRequest(new { message = "Student cannot be null" });
        try
        {
            var result = StudentService.AddStudent(student);
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
    [HttpPut]
    public IActionResult UpdateStudent([FromBody] StudentDto student)
    {
        if (student is null)
            return BadRequest(new { message = "Student canno be null" });
        try
        {
            var updatedStudent = StudentService.UpdateStudent(student);
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
    [HttpDelete("id")]
    public IActionResult DeleteStudent(int id)
    {
        if (StudentService.GetStudentById(id) is null)
            return BadRequest("Cannot found student with this Id");

        return Ok(StudentService.DeleteStudent(id));
    }
}
