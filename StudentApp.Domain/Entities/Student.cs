namespace StudentApp.Domain.Entities;

/// <summary>
/// Represents a student.
/// </summary>
public sealed record class Student
{
    public required int Id { get; set; }
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
}
