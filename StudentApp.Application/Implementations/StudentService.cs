namespace StudentApp.Application.Implementations;

using StudentApp.Application.Models;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Repositories;


public interface IStudentService
{
    StudentDto[] GetAllStudents();
    StudentDto GetStudentById(int id);
    int AddStudent(StudentDto student);
    StudentDto UpdateStudent(StudentDto student);
    Boolean DeleteStudent(int id);
}

/// <summary>
/// Provides operations for retrieving student data using a student repository.
/// </summary>
internal class StudentService
{
    private readonly IStudentRepository StudentRepository;

    /// <summary>
    /// Initializes a new instance of the StudentService class using the student repository.
    /// </summary>
    /// <param name="_studentRepository">The repository used to access data. Cannot be null.</param>
    public StudentService(IStudentRepository _studentRepository)
    {
        StudentRepository = _studentRepository;
    }

    /// <summary>
    /// Retrieves all students as an array of data transfer objects.
    /// </summary>
    /// <returns>An array of <see cref="StudentDto"/> objects representing all students</returns>
    public StudentDto[] GetAllStudents()
    {
        var students = StudentRepository.getAllStudents();
        StudentDto[] studentsDto = [];

        foreach (Student student in students)
        {
            var studentToAdd = new StudentDto(student.Id, student.FirstName,
                student.LastName);
            studentsDto.Append(studentToAdd);
        }

        return studentsDto;
    }

    /// <summary>
    /// Retrieves a student by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student to retrieve.</param>
    /// <returns>A <see cref="StudentDto"/> representing the student with the specified identifier.</returns>
    public StudentDto GetStudentById(int id)
    {
        var student = StudentRepository.getStudentById(id);
        return new StudentDto(student.Id, student.FirstName, student.LastName);
    }

    /// <summary>
    /// Adds a new student to the repository using the provided student data transfer object.
    /// </summary>
    /// <param name="student">An object containing the first and last name of the student to add. Cannot be null.</param>
    /// <returns>The unique identifier of the newly added student.</returns>
    public int AddStudent(StudentDto student)
    {
        var studentToAdd = new Student()
        {
            FirstName = student.FirstName,
            LastName = student.LastName
        };

        return StudentRepository.AddStudent(studentToAdd);
    }

    /// <summary>
    /// Updates the details of an existing student using the provided student data.
    /// </summary>
    /// <remarks>If the specified student Id does not exist, the update operation may fail</remarks>
    /// <param name="student">An object containing the updated information for the student.</param>
    /// <returns>A StudentDto representing the student after the update has been applied.</returns>
    public StudentDto UpdateStudent(StudentDto student)
    {
        var studentToUpdate = new Student()
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName
        };

        var updatedStudent = StudentRepository.UpdateStudent(studentToUpdate);

        return new StudentDto(updatedStudent.Id, updatedStudent.FirstName, updatedStudent.LastName);
    }


    /// <summary>
    /// Deletes the student record with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student to delete.</param>
    /// <returns>true if the student was successfully deleted; otherwise, false.</returns>
    public Boolean DeleteStudent(int id)
    {
        return StudentRepository.DeleteStudent(id);
    }
}
