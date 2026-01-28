namespace StudentApp.Application.Tests.Implementation;

using NSubstitute;
using StudentApp.Application.Implementations;
using StudentApp.Application.Models;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Abstractions;
using System.Threading;

public class StudentServiceTests
{
    private readonly IStudentRepository mockStudentRepository = Substitute.For<IStudentRepository>();
    private readonly StudentService studentService;

    public StudentServiceTests()
        => this.studentService = new StudentService(mockStudentRepository);


    [Fact]
    public async Task GetAllStudents_WhenEmpty_ReturnEmptyList()
    {
        // Arrange
        mockStudentRepository.GetAllStudentsAsync(CancellationToken.None).Returns([]);

        // Act
        var result = await studentService.GetAllStudentsAsync(CancellationToken.None);

        // Assert

        Assert.Equal([], result);
    }

    [Fact]
    public async Task GetAllStudent_WhenFill_ReturnStudentDtoList()
    {
        // Arrange
        mockStudentRepository.GetAllStudentsAsync(CancellationToken.None).Returns([
            new Student {Id = 1, FirstName = "firstName", LastName = "lastName"},
            new Student {Id = 2, FirstName = "firstNameTwo", LastName = "lastNameTwo"},
            ]);

        // Act
        var result = await studentService.GetAllStudentsAsync(CancellationToken.None);

        // Assert
        Assert.Equal(
            [
            new StudentDto(1, "firstName", "lastName"),
            new StudentDto(2, "firstNameTwo", "lastNameTwo"),
            ], result);
    }

    [Fact]
    public async Task GetStudentById_WhenStudentNotFound_ReturnNull()
    {
        // Arrange
        mockStudentRepository.GetStudentByIdAsync(1, CancellationToken.None).Returns((Student?)null);

        // Act
        var result = await studentService.GetStudentByIdAsync(1, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetStudentById_WhenStudentFound_ReturnStudent()
    {
        // Arrange
        mockStudentRepository.GetStudentByIdAsync(1, CancellationToken.None).Returns(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" });

        // Act
        var result = await studentService.GetStudentByIdAsync(1, CancellationToken.None);

        // Assert
        Assert.Equal(new StudentDto(1, "firstName", "lastName"), result);
    }

    [Fact]
    public async Task AddStudentAsync_WhenStudentIsOk_ReturnId()
    {
        // Arrange
        mockStudentRepository.AddStudentAsync(new Student { FirstName = "firstName", LastName = "lastName" }, CancellationToken.None).Returns(1);

        // Act
        var result = await studentService.AddStudentAsync(new StudentDto(null, "firstName", "lastName"), CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateStudentAsync_WhenIdChanged_ReturnNull()
    {
        // Arrange
        mockStudentRepository.UpdateStudentAsync(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" }, CancellationToken.None)
            .Returns(new Student { Id = 1, FirstName = "firstNameUpdated", LastName = "lastName" });

        // Act
        var result = await studentService.UpdateStudentAsync(2, new StudentDto(1, "firstNameUpdated", "lastName"), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task UpdateStudentAsync_WhenStudentDoesntExist_ReturnNull()
    {
        // Arrange
        mockStudentRepository.GetStudentByIdAsync(1, CancellationToken.None).Returns((Student?)null);
        mockStudentRepository.UpdateStudentAsync(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" }, CancellationToken.None).Returns(new Student { Id = 1, FirstName = "firstNameUpdated", LastName = "lastName" });

        // Act
        var result = await studentService.UpdateStudentAsync(1, new StudentDto(1, "firstNameUpdated", "lastName"), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateStudentAsync_WhenIdDontChanged_ReturnUpdatedStudent()
    {
        // Arrange
        mockStudentRepository.GetStudentByIdAsync(1, CancellationToken.None)
            .Returns(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" });
        mockStudentRepository.UpdateStudentAsync(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" }, CancellationToken.None)
            .Returns(new Student { Id = 1, FirstName = "firstNameUpdated", LastName = "lastName" });

        // Act
        var result = await studentService.UpdateStudentAsync(1, new StudentDto(1, "firstNameUpdated", "lastName"), CancellationToken.None);

        // Assert
        Assert.Equal(new StudentDto(1, "firstNameUpdated", "lastName"), result);
    }

    [Fact]
    public async Task DeleteStudentAsync_WhenStudentExists_ReturnTrue()
    {
        // Arrange
        mockStudentRepository.DeleteStudentAsync(1, CancellationToken.None).Returns(true);

        // Act
        var result = await studentService.DeleteStudentAsync(1, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteStudentAsync_WhenStudentDoesntExist_ReturnFalse()
    {
        // Arrange
        mockStudentRepository.DeleteStudentAsync(1, CancellationToken.None).Returns(false);

        // Act
        var result = await studentService.DeleteStudentAsync(1, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}
