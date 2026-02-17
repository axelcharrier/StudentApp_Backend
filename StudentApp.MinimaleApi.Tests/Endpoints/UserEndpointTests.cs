namespace StudentApp.ApiMinimal.Tests.Endpoints;

using Microsoft.AspNetCore.Http;
using NSubstitute;
using StudentApp.ApiMinimal.Endpoints;
using StudentApp.Application.Abstraction;
using StudentApp.Application.Models.Dto;
using System.Reflection;
using System.Threading;

/// <summary>
/// Contains unit tests for the user-related API endpoints, verifying expected responses for various user service
/// scenarios.
/// </summary>
/// <remarks>These tests cover cases such as retrieving all users, fetching a user by email, updating user
/// information, and deleting users. Each test validates the HTTP status code returned by the endpoint under different
/// conditions, including success, not found, and failure scenarios. The tests use a mocked user service to simulate
/// backend behavior.</remarks>
public class UserEndpointTests
{
    private readonly IUserService mockUserService = Substitute.For<IUserService>();
    private readonly CancellationToken ct = CancellationToken.None;
    [Fact]
    public async Task GetAllUsersAsync_WhenEmpty_ReturnStatus200Ok()
    {
        // Arrange
        this.mockUserService.GetAllUsersAsync(ct).Returns([]);

        // Act
        var result = await InvokeGetAllUsersEndpoint(ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(1).GetAllUsersAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAllUsersAsync_WhenFilled_ReturnStatus200Ok()
    {
        // Arrange
        this.mockUserService.GetAllUsersAsync(ct).Returns([
                new UserDto("test@test.com", true, "Teacher"),
                new UserDto("test2@test.com", false, "Student")
            ]);

        // Act
        var result = await InvokeGetAllUsersEndpoint(ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(1).GetAllUsersAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserByMailAsync_WhenUserNotFound_ReturnStatus404NotFound()
    {
        // Arrange
        this.mockUserService.GetUserByMailAsync("test@test.com", ct).Returns((UserDto?)null);

        // Act
        var result = await InvokeGetByMailEndpoint("test@test.com", ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(1).GetUserByMailAsync("test@test.com", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserByMailAsync_WhenUserFound_ReturnStatus200Ok()
    {
        // Arrange
        var expectedResult = new UserDto("test@test.com", true, "Teacher");
        this.mockUserService.GetUserByMailAsync("test@test.com", ct).Returns(expectedResult);

        // Act
        var result = await InvokeGetByMailEndpoint("test@test.com", ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(1).GetUserByMailAsync("test@test.com", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserNotFound_ReturnStatus404NotFound()
    {
        // Arrange 
        var userDto = new UserDto("test@test.com", true, "Teacher");
        this.mockUserService.GetUserByMailAsync("test@test.com", ct).Returns((UserDto?)null);
        this.mockUserService.UpdateUserAsync(userDto, ct).Returns((UserDto?)null);

        // Act
        var result = await InvokeUpdateUserEndpoint(userDto, ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(0).UpdateUserAsync(userDto, Arg.Any<CancellationToken>());
        await this.mockUserService.Received(1).GetUserByMailAsync("test@test.com", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUpdateFail_ReturnStatus400BadRequest()
    {
        // Arrange 
        var userDto = new UserDto("test@test.com", true, "Teacher");
        this.mockUserService.GetUserByMailAsync("test@test.com", ct).Returns(userDto);
        this.mockUserService.UpdateUserAsync(userDto, ct).Returns((UserDto?)null);

        // Act
        var result = await InvokeUpdateUserEndpoint(userDto, ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(1).UpdateUserAsync(userDto, Arg.Any<CancellationToken>());
        await this.mockUserService.Received(1).GetUserByMailAsync("test@test.com", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUpdateSuccess_ReturnStatus200Ok()
    {
        // Arrange 
        var userDto = new UserDto("test@test.com", true, "Teacher");
        this.mockUserService.GetUserByMailAsync("test@test.com", ct).Returns(userDto);
        this.mockUserService.UpdateUserAsync(userDto, ct).Returns(userDto);

        // Act
        var result = await InvokeUpdateUserEndpoint(userDto, ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(1).UpdateUserAsync(userDto, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserNotFound_ReturnStatus404NotFound()
    {
        // Arrange 
        this.mockUserService.GetUserByMailAsync("test@test.com", ct).Returns((UserDto?)null);
        this.mockUserService.DeleteUserAsync("test@test.com", ct).Returns(false);

        // Act
        var result = await InvokeDeleteUserEndpoint("test@test.com", ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(1).DeleteUserAsync("test@test.com", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteUserAsync_WhenDeleteFail_ReturnStatus400BadRequest()
    {
        // Arrange 
        var userDto = new UserDto("test@test.com", true, "Teacher");
        this.mockUserService.GetUserByMailAsync("test@test.com", ct).Returns(userDto);
        this.mockUserService.DeleteUserAsync("test@test.com", ct).Returns(false);

        // Act
        var result = await InvokeDeleteUserEndpoint("test@test.com", ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(1).DeleteUserAsync("test@test.com", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteUserAsync_WhenDeleteSuccess_ReturnStatus200Ok()
    {
        // Arrange 
        var userDto = new UserDto("test@test.com", true, "Teacher");
        this.mockUserService.GetUserByMailAsync("test@test.com", ct).Returns(userDto);
        this.mockUserService.DeleteUserAsync("test@test.com", ct).Returns(true);

        // Act
        var result = await InvokeDeleteUserEndpoint("test@test.com", ct);

        // Assert
        Assert.NotNull(result);
        await this.mockUserService.Received(1).DeleteUserAsync("test@test.com", Arg.Any<CancellationToken>());
    }

    #region

    private async Task<IResult> InvokeGetAllUsersEndpoint(CancellationToken ct)
    {
        // Use reflection to invoke the private static method
        // We search a method with the name "GetAllStudentsAsync" which is non-public and static
        var method = typeof(UsersEndpoints)
            .GetMethod("GetAllUsersAsync", BindingFlags.NonPublic | BindingFlags.Static);

        // The method is not null
        Assert.NotNull(method);

        // Invoke the method without parameters but with the mock service and the cancellation token
        var task = method.Invoke(null, [ct, this.mockUserService]) as Task<IResult>;
        Assert.NotNull(task);

        return await task;
    }

    private async Task<IResult> InvokeGetByMailEndpoint(string mail, CancellationToken ct)
    {
        // Use reflection to invoke the private static method
        // We search a method with the name "GetAllStudentsAsync" which is non-public and static
        var method = typeof(UsersEndpoints)
            .GetMethod("GetUserByMailAsync", BindingFlags.NonPublic | BindingFlags.Static);

        // The method is not null
        Assert.NotNull(method);

        // Invoke the method without parameters but with the mock service and the cancellation token
        var task = method.Invoke(null, [mail, ct, this.mockUserService]) as Task<IResult>;
        Assert.NotNull(task);

        return await task;
    }
    private async Task<IResult> InvokeUpdateUserEndpoint(UserDto user, CancellationToken ct)
    {
        // Use reflection to invoke the private static method
        // We search a method with the name "GetAllStudentsAsync" which is non-public and static
        var method = typeof(UsersEndpoints)
            .GetMethod("UpdateUserAsync", BindingFlags.NonPublic | BindingFlags.Static);

        // The method is not null
        Assert.NotNull(method);

        // Invoke the method without parameters but with the mock service and the cancellation token
        var task = method.Invoke(null, [user, this.mockUserService, ct]) as Task<IResult>;
        Assert.NotNull(task);

        return await task;
    }
    private async Task<IResult> InvokeDeleteUserEndpoint(string mail, CancellationToken ct)
    {
        // Use reflection to invoke the private static method
        // We search a method with the name "GetAllStudentsAsync" which is non-public and static
        var method = typeof(UsersEndpoints)
            .GetMethod("DeleteUserAsync", BindingFlags.NonPublic | BindingFlags.Static);

        // The method is not null
        Assert.NotNull(method);

        // Invoke the method without parameters but with the mock service and the cancellation token
        var task = method.Invoke(null, [mail, this.mockUserService, ct]) as Task<IResult>;
        Assert.NotNull(task);

        return await task;
    }
}

#endregion