namespace StudentApp.Domain.Models;

public sealed record class Student
{
    public required int Id { get; set; }
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
}
