namespace StudentApp.Application.Tests.Implementation;

using NSubstitute;
using StudentApp.Application.Implementations;
using StudentApp.Application.Models;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Abstractions;

public class StudentServiceTests
{
    [Fact]
    public async Task GetAllStudents_ReturnEmptyList_WhenEmpty()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.GetAllStudentsAsync(ct).Returns([]);
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.GetAllStudentsAsync(ct);

        // Assert

        Assert.Equal([], result);
    }

    [Fact]
    public async Task GetAllStudent_ReturnStudentDtoList_WhenFill()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.GetAllStudentsAsync(ct).Returns([
            new Student {Id = 1, FirstName = "firstName", LastName = "lastName"},
            new Student {Id = 2, FirstName = "firstNameTwo", LastName = "lastNameTwo"},
            ]);
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.GetAllStudentsAsync(ct);

        // Assert
        Assert.Equal(
            [
            new StudentDto(1, "firstName", "lastName"),
            new StudentDto(2, "firstNameTwo", "lastNameTwo"),
            ], result);
    }

    [Fact]
    public async Task GetStudentById_ReturnNull_WhenStudentNotFound()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.GetStudentByIdAsync(1, ct).Returns((Student?)null);
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.GetStudentByIdAsync(1, ct);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetStudentById_ReturnStudent_WhenStudentFound()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.GetStudentByIdAsync(1, ct).Returns(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" });
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.GetStudentByIdAsync(1, ct);

        // Assert
        Assert.Equal(new StudentDto(1, "firstName", "lastName"), result);
    }

    [Fact]
    public async Task AddStudentAsync_ReturnId_WhenStudentIsOk()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.AddStudentAsync(new Student { FirstName = "firstName", LastName = "lastName" }, ct).Returns(1);
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.AddStudentAsync(new StudentDto(null, "firstName", "lastName"), ct);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task UpdateStudentAsync_ReturnNull_WhenIdChanged()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.UpdateStudentAsync(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" }, ct).Returns(new Student { Id = 1, FirstName = "firstNameUpdated", LastName = "lastName" });
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.UpdateStudentAsync(2, new StudentDto(1, "firstNameUpdated", "lastName"), ct);

        // Assert
        Assert.Null(result);
    }


    [Fact]
    public async Task UpdateStudentAsync_ReturnNull_WhenStudentDoesntExist()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.GetStudentByIdAsync(1, ct).Returns((Student?)null);
        mockStudentRepository.UpdateStudentAsync(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" }, ct).Returns(new Student { Id = 1, FirstName = "firstNameUpdated", LastName = "lastName" });
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.UpdateStudentAsync(1, new StudentDto(1, "firstNameUpdated", "lastName"), ct);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateStudentAsync_ReturnUpdatedStudent_WhenIdDontChanged()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.GetStudentByIdAsync(1, ct).Returns(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" });
        mockStudentRepository.UpdateStudentAsync(new Student { Id = 1, FirstName = "firstName", LastName = "lastName" }, ct).Returns(new Student { Id = 1, FirstName = "firstNameUpdated", LastName = "lastName" });
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.UpdateStudentAsync(1, new StudentDto(1, "firstNameUpdated", "lastName"), ct);

        // Assert
        Assert.Equal(new StudentDto(1, "firstNameUpdated", "lastName"), result);
    }

    [Fact]
    public async Task DeleteStudentAsync_ReturnTrue_WhenStudentExists()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.DeleteStudentAsync(1, ct).Returns(true);
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.DeleteStudentAsync(1, ct);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteStudentAsync_ReturnFalse_WhenStudentDoesntExist()
    {
        // Arrange
        var ct = new CancellationToken();
        var mockStudentRepository = Substitute.For<IStudentRepository>();
        mockStudentRepository.DeleteStudentAsync(1, ct).Returns(false);
        var studentService = new StudentService(mockStudentRepository);

        // Act
        var result = await studentService.DeleteStudentAsync(1, ct);

        // Assert
        Assert.False(result);
    }
}
