using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;
using StudentApp.Application.Models;
using StudentApp.Domain.Models;
using StudentApp.Infrastructure.Repositories;

namespace StudentApp.Application.Implementations
{
    public interface IStudentService
    {
        StudentDto[] GetAllStudents();
        StudentDto GetStudent(int id);
        int AddStudent(StudentDto student);
        StudentDto UpdateStudent(StudentDto student);
        Boolean DeleteStudent(int id);
    }
    internal class StudentService
    {
        private readonly IStudentRepository StudentRepository;
        
        public StudentService(IStudentRepository _studentRepository)
        {
            StudentRepository = _studentRepository;
        }

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
    }
}
