namespace StudentApp.Application.Implementations;

using StudentApp.Application.Abstraction;
using StudentApp.Application.Models;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Abstractions;


/// <inheritdoc/>
public sealed class StudentService(IStudentRepository studentRepository) : IStudentService
{

    public async Task<StudentDto[]> GetAllStudents(CancellationToken ct)
    {
        var students = await studentRepository.getAllStudents(ct);
        var result = students.Select(stu => new StudentDto(stu.Id, stu.FirstName, stu.LastName)).ToArray();

        return result;
    }


    public async Task<StudentDto?> GetStudentById(int id, CancellationToken ct)
    {
        var student = await studentRepository.getStudentById(id, ct);
        if (student is null)
            return null;
        return new StudentDto(student.Id, student.FirstName, student.LastName);
    }

    public async Task<int> AddStudent(StudentDto student, CancellationToken ct)
    {
        var studentToAdd = new Student()
        {
            FirstName = student.FirstName,
            LastName = student.LastName
        };

        return await studentRepository.AddStudent(studentToAdd, ct);
    }


    public async Task<StudentDto?> UpdateStudent(StudentDto student, CancellationToken ct)
    {
        //récupération modification sauvegarde. CHANGEMENT A FAIRE
        var studentToUpdate = await studentRepository.getStudentById(student.Id, ct);

        if (studentToUpdate is null)
            return null;

        studentToUpdate.FirstName = student.FirstName;
        studentToUpdate.LastName = student.LastName;

        await studentRepository.UpdateStudent(studentToUpdate, ct);

        return student;
    }

    public async Task<Boolean> DeleteStudent(int id, CancellationToken ct)
    {
        return await studentRepository.DeleteStudent(id, ct);
    }
}
