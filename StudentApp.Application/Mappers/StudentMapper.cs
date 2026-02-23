namespace StudentApp.Application.Mappers;

using Riok.Mapperly.Abstractions;
using StudentApp.Application.Models.Dto;
using StudentApp.Domain.Entities;

[Mapper]
public static partial class StudentMapper
{
    [MapProperty(nameof(Student.FirstName), nameof(StudentDto.FirstName))]
    [MapProperty(nameof(Student.LastName), nameof(StudentDto.LastName))]
    [MapProperty(nameof(Student.Id), nameof(StudentDto.Id))]

    public static partial StudentDto ToStudentDto(this Student student);
}
