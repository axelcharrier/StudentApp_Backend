namespace StudentApp.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Persistence;

public interface IStudentRepository
{
    IEnumerable<Student> getAllStudents();
    Student? getStudentById(int id);
    int AddStudent(Student studentToAdd);
    Student UpdateStudent(Student studentToUpdate);
    Boolean DeleteStudent(int id);

}

internal class StudentRepository
{
    private readonly AppDbContext Context;

    public StudentRepository(AppDbContext _context)
    {
        Context = _context;
    }

    public IEnumerable<Student> getAllStudents()
    {
        var list = Context.Students.ToList();
        return list;
    }

    public Student? getStudentById(int id)
    {
        var studentToReturn = Context.Students.Find(id);
        return studentToReturn;
    }

    public int AddStudent(Student studentToAdd)
    {
        Context.Students.Add(studentToAdd);
        Context.SaveChanges();
        return studentToAdd.Id;
    }

    public Student UpdateStudent(Student studentToUpdate)
    {
        Context.Update(studentToUpdate);
        Context.SaveChanges();
        return studentToUpdate;
    }

    public Boolean DeleteStudent(int id)
    {
        var studentToDelete = Context.Students.Find(id);
        if (studentToDelete != null)
        {
            Context.Students.Remove(studentToDelete);
            Context.SaveChanges();
        }
        return true;
    }
}
