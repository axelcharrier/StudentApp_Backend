namespace StudentApp.Infrastructure.Abstractions;

using StudentApp.Domain.Entities;
using System;
using System.Collections.Generic;

public interface IStudentRepository
{
    /// <summary>
    /// Asynchronously retrieves all students from the data store.
    /// </summary>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all students.</returns>
    Task<IEnumerable<Student>> GetAllStudentsAsync(CancellationToken ct);

    /// <summary>
    /// Asynchronously retrieves a student with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the student with the specified
    /// identifier, or <see langword="null"/> if no matching student is found.</returns>
    Task<Student?> GetStudentByIdAsync(int? id, CancellationToken ct);

    /// <summary>
    /// Asynchronously adds a new student to the data store.
    /// </summary>
    /// <param name="studentToAdd">The student entity to add. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the unique identifier of the newly
    /// added student.</returns>
    Task<int> AddStudentAsync(Student studentToAdd, CancellationToken ct);

    /// <summary>
    /// Asynchronously updates the specified student entity in the data store.
    /// </summary>
    /// <param name="studentToUpdate">The student entity to update. The entity must be tracked by the current context and contain the updated values.
    /// Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated student entity.</returns>
    Task<Student> UpdateStudentAsync(Student studentToUpdate, CancellationToken ct);

    /// <summary>
    /// Deletes the student with the specified identifier from the data store asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the student to delete.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the student was
    /// found and deleted; otherwise, <see langword="false"/>.</returns>
    Task<Boolean> DeleteStudentAsync(int id, CancellationToken ct);

}
