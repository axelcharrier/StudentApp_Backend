namespace StudentApp.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Abstractions;
using StudentApp.Infrastructure.Persistence;

/// <inheritdoc/>
public class StudentRepository(AppDbContext context) : IStudentRepository
{

    public async Task<IEnumerable<Student>> getAllStudents(CancellationToken ct)
        => await context.Students.ToListAsync(ct).ConfigureAwait(false);


    public async Task<Student?> getStudentById(int id, CancellationToken ct)
        => await context.Students.FirstOrDefaultAsync(stu => stu.Id == id, ct).ConfigureAwait(false);

    public async Task<int> AddStudent(Student studentToAdd, CancellationToken ct)
    {
        await context.Students.AddAsync(studentToAdd, ct).ConfigureAwait(false);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);
        return studentToAdd.Id;
    }

    public async Task<Student> UpdateStudent(Student studentToUpdate, CancellationToken ct)
    {
        context.Update(studentToUpdate);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);
        return studentToUpdate;
    }

    public async Task<Boolean> DeleteStudent(int id, CancellationToken ct)
    {
        var studentToDelete = await context.Students.FirstOrDefaultAsync(stu => stu.Id == id, ct).ConfigureAwait(false);
        if (studentToDelete != null)
        {
            context.Students.Remove(studentToDelete);
            await context.SaveChangesAsync(ct).ConfigureAwait(false);
            return true;
        }
        return false;
    }
}
