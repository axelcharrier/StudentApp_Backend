namespace StudentApp.ApiMinimal.Tests.Endpoints;

using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using StudentApp.ApiMinimal.Endpoints;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Models;
using System.Reflection;
using System.Threading;

/// <summary>
/// Contains unit tests for the Students API endpoints, verifying correct behavior for student-related operations such
/// as retrieval, creation, update, and deletion.
/// </summary>
public class StudentsEndpointTests
{
    private readonly IStudentService mockStudentService = Substitute.For<IStudentService>();

    [Fact]
    public async Task GetAllAsync_WhenEmpty_ReturnStatus200Ok()
    {
        // Arrange
        StudentDto[] expectedResult = [];
        this.mockStudentService.GetAllStudentsAsync(CancellationToken.None).Returns(expectedResult);

        // Act
        var result = await InvokeGetAllEndpoint(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        await this.mockStudentService.Received(1).GetAllStudentsAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAllAsync_WhenFilled_ReturnStatus200Ok()
    {
        // Arrange
        StudentDto[] expectedResult = [new StudentDto(1, "firstname", "lastname")];
        this.mockStudentService.GetAllStudentsAsync(CancellationToken.None).Returns(expectedResult);

        // Act
        var result = await InvokeGetAllEndpoint(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        await this.mockStudentService.Received(1).GetAllStudentsAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetByIdAsync_WhenStudentDoesntExist_ReturnStatus404NotFound()
    {
        // Arrange
        StudentDto? expectedResult = null;
        this.mockStudentService.GetStudentByIdAsync(1, CancellationToken.None).Returns(expectedResult);

        // Act
        var result = await InvokeGetByIdEndpoint(1, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        await this.mockStudentService.Received(1).GetStudentByIdAsync(1, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetByIdAsync_WhenStudentExists_ReturnStatus200Ok()
    {
        // Arrange
        StudentDto? expectedResult = new StudentDto(1, "firstName", "lastName");
        this.mockStudentService.GetStudentByIdAsync(1, CancellationToken.None).Returns(expectedResult);

        // Act
        var result = await InvokeGetByIdEndpoint(1, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        await this.mockStudentService.Received(1).GetStudentByIdAsync(1, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddAsync_WhenStudentIsOk_ReturnStatus201Created()
    {
        // Arrange
        var expectedResult = 1;
        var studentToAdd = new StudentDto(null, "firstName", "lastName");
        this.mockStudentService.AddStudentAsync(studentToAdd, CancellationToken.None).Returns(expectedResult);

        // Act
        var result = await InvokeAddEndpoint(studentToAdd, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        await this.mockStudentService.Received(1).AddStudentAsync(studentToAdd, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_WhenStudentIdChangedOrStudentDoesntExist_ReturnStatus400BadRequest()
    {
        // Arrange
        StudentDto? expectedResult = null;
        var studentToUpdate = new StudentDto(1, "firstName", "lastName");
        this.mockStudentService.UpdateStudentAsync(2, studentToUpdate, CancellationToken.None).Returns(expectedResult);

        // Act
        var result = await InvokeUpdateEndpoint(2, studentToUpdate, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        await this.mockStudentService.Received(1).UpdateStudentAsync(2, studentToUpdate, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_WhenStudentIdDoesntChanged_ReturnStatus200Ok()
    {
        // Arrange
        var studentToUpdate = new StudentDto(2, "firstName", "lastName");
        StudentDto? expectedResult = studentToUpdate;
        this.mockStudentService.UpdateStudentAsync(2, studentToUpdate, CancellationToken.None).Returns(expectedResult);

        // Act
        var result = await InvokeUpdateEndpoint(2, studentToUpdate, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        await this.mockStudentService.Received(1).UpdateStudentAsync(2, studentToUpdate, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_WhenStudentExists_ReturnStatus200Ok()
    {
        // Arrange
        Boolean expectedResult = true;
        this.mockStudentService.GetStudentByIdAsync(2, CancellationToken.None).Returns(new StudentDto(2, "firstName", "lastName"));
        this.mockStudentService.DeleteStudentAsync(2, CancellationToken.None).Returns(expectedResult);

        // Act
        var result = await InvokeDeleteEndpoint(2, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        await this.mockStudentService.Received(1).DeleteStudentAsync(2, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_WhenStudentDoesntExist_ReturnStatus400BadRequest()
    {
        // Arrange
        Boolean expectedResult = false;
        this.mockStudentService.GetStudentByIdAsync(2, CancellationToken.None).Returns(new StudentDto(2, "firstName", "lastName"));
        this.mockStudentService.DeleteStudentAsync(2, CancellationToken.None).Returns(expectedResult);

        // Act
        var result = await InvokeDeleteEndpoint(2, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        await this.mockStudentService.Received(1).DeleteStudentAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
    }


    #region Invoke

    /// <summary>
    /// Invokes the private endpoint to retrieve all student records asynchronously for testing purposes.
    /// </summary>
    private async Task<IResult> InvokeGetAllEndpoint(CancellationToken cancellationToken)
    {
        // Use reflection to invoke the private static method
        // We search a method with the name "GetAllStudentsAsync" which is non-public and static
        var method = typeof(StudentsEndpoints)
            .GetMethod("GetAllAsync", BindingFlags.NonPublic | BindingFlags.Static);

        // The method is not null
        Assert.NotNull(method);

        // Invoke the method without parameters but with the mock service and the cancellation token
        var task = method.Invoke(null, [cancellationToken, this.mockStudentService]) as Task<IResult>;
        Assert.NotNull(task);

        return await task;
    }

    /// <summary>
    /// Invokes the private endpoint to retrieve a student records by his id asynchronously for testing purposes.
    /// </summary>
    private async Task<IResult> InvokeGetByIdEndpoint(int id, CancellationToken cancellationToken)
    {
        // Use reflection to invoke the private static method
        // We search a method with the name "GetByIdAsync" which is non-public and static
        var method = typeof(StudentsEndpoints)
            .GetMethod("GetByIdAsync", BindingFlags.NonPublic | BindingFlags.Static);

        // The method is not null
        Assert.NotNull(method);

        // Invoke the method without parameters but with the mock service and the cancellation token
        var task = method.Invoke(null, [cancellationToken, id, this.mockStudentService]) as Task<IResult>;
        Assert.NotNull(task);

        return await task;
    }

    /// <summary>
    /// Invokes the private endpoint to update a student records asynchronously for testing purposes.
    /// </summary>
    private async Task<IResult> InvokeUpdateEndpoint(int id, StudentDto student, CancellationToken cancellationToken)
    {
        // Use reflection to invoke the private static method
        // We search a method with the name "UpdateStudentAsync" which is non-public and static
        var method = typeof(StudentsEndpoints)
            .GetMethod("UpdateStudentAsync", BindingFlags.NonPublic | BindingFlags.Static);

        // The method is not null
        Assert.NotNull(method);

        // Invoke the method without parameters but with the mock service and the cancellation token
        var task = method.Invoke(null, [cancellationToken, id, student, this.mockStudentService]) as Task<IResult>;
        Assert.NotNull(task);

        return await task;
    }

    /// <summary>
    /// Invokes the private endpoint to add a student records asynchronously for testing purposes.
    /// </summary>
    private async Task<IResult> InvokeAddEndpoint(StudentDto student, CancellationToken cancellationToken)
    {
        // Use reflection to invoke the private static method
        // We search a method with the name "AddStudentAsync" which is non-public and static
        var method = typeof(StudentsEndpoints)
            .GetMethod("AddStudentAsync", BindingFlags.NonPublic | BindingFlags.Static);

        // The method is not null
        Assert.NotNull(method);

        // Invoke the method without parameters but with the mock service and the cancellation token
        var task = method.Invoke(null, [cancellationToken, student, this.mockStudentService]) as Task<IResult>;
        Assert.NotNull(task);

        return await task;
    }

    /// <summary>
    /// Invokes the private endpoint to delete a student records asynchronously for testing purposes.
    /// </summary>
    private async Task<IResult> InvokeDeleteEndpoint(int id, CancellationToken cancellationToken)
    {
        // Use reflection to invoke the private static method
        // We search a method with the name "DeleteStudentAsync" which is non-public and static
        var method = typeof(StudentsEndpoints)
            .GetMethod("DeleteStudentAsync", BindingFlags.NonPublic | BindingFlags.Static);

        // The method is not null
        Assert.NotNull(method);

        // Invoke the method without parameters but with the mock service and the cancellation token
        var task = method.Invoke(null, [cancellationToken, id, this.mockStudentService]) as Task<IResult>;
        Assert.NotNull(task);

        return await task;
    }

    #endregion
}
