using StudentApp.Application.Models;

namespace StudentApp.Application.Abstraction;


public interface IStudentService
{

    /// <summary>
    /// Asynchronously adds a new student to the system.
    /// </summary>
    /// <param name="student">The student information to add. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the newly
    /// added student.</returns>
    /// 
    Task<int> AddStudentAsync(StudentDto student, CancellationToken ct);
    /// <summary>
    /// Deletes the student with the specified identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the student to delete.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the
    /// student was deleted successfully; otherwise, <see langword="false"/>.</returns>
    /// 
    Task<bool> DeleteStudentAsync(int id, CancellationToken ct);
    /// <summary>
    /// Asynchronously retrieves all students.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of StudentDto objects
    /// representing all students. The array is empty if no students are found.</returns>
    Task<StudentDto[]> GetAllStudentsAsync(CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves a student by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="StudentDto"/>
    /// representing the student if found; otherwise, <see langword="null"/>.</returns>
    Task<StudentDto?> GetStudentByIdAsync(int id, CancellationToken ct);

    /// <summary>
    /// Updates the information of an existing student asynchronously.
    /// </summary>
    /// <param name="student">The student data to update. Must contain a valid student identifier and the updated information. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the update operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="StudentDto"/> with the
    /// updated student information if the update is successful; otherwise, <see langword="null"/> if the student does
    /// not exist.</returns>
    Task<StudentDto?> UpdateStudentAsync(int id, StudentDto student, CancellationToken ct);
}
