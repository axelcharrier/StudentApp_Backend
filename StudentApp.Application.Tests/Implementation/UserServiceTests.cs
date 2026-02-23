using Microsoft.AspNetCore.Identity;
using NSubstitute;
using StudentApp.Application.Implementations;
using StudentApp.Application.Models.Dto;
using StudentApp.Domain.Entities;
using StudentApp.Infrastructure.Abstractions;

namespace StudentApp.Application.Tests.Implementation;

/// <summary>
/// Contains unit tests for the UserService class, verifying user-related operations such as retrieval, update, and
/// deletion.
/// </summary>
/// <remarks>This test class uses mocked dependencies to isolate and validate the behavior of UserService methods
/// under various scenarios. Tests are organized to cover both successful and failure cases for user management
/// operations.</remarks>
public class UserServiceTests
{
    private readonly IUserRepository mockUserRepository = Substitute.For<IUserRepository>();
    private readonly UserManager<IdentityUser> mockUserManager = Substitute.For<UserManager<IdentityUser>>(Substitute.For<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
    private readonly UserService userService;
    private readonly CancellationToken ct = CancellationToken.None;

    public UserServiceTests()
        => this.userService = new UserService(mockUserRepository, mockUserManager);

    [Fact]
    public async Task GetAllUser_WhenEmpty_ReturnEmptyList()
    {
        // Arrange 
        mockUserRepository.GetAllUsersAsync(ct).Returns([]);

        // Act
        var result = await userService.GetAllUsersAsync(ct);

        // Assert
        Assert.Equal([], result);
    }

    [Fact]
    public async Task GetAllUser_WhenFill_ReturnUserDtoList()
    {
        // Arrange
        mockUserRepository.GetAllUsersAsync(ct).Returns([
            new User {Mail = "test1@test.com", IsMailConfirmed = true, Role = "Student"},
            new User {Mail = "test2@test.com", IsMailConfirmed = false, Role = "Teacher"},
            ]);

        // Act
        var result = await userService.GetAllUsersAsync(ct);

        // Assert 
        Assert.Equal([
            new UserDto("test1@test.com", true, "Student"),
            new UserDto("test2@test.com", false, "Teacher")
            ], result);
    }

    [Fact]
    public async Task GetUserByMail_WhenUserNotFound_ReturnNull()
    {
        // Arrange
        mockUserRepository.GetUserByMailAsync("test1@test.com", ct).Returns((User?)null);

        // Act
        var result = await userService.GetUserByMailAsync("test1@test.com", ct);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByMail_WhenUserFound_ReturnUser()
    {
        // Arrange 
        mockUserRepository.GetUserByMailAsync("test1@test.com", ct).Returns(new User { Mail = "test1@test.com", IsMailConfirmed = true, Role = "Student" });

        // Act
        var result = await userService.GetUserByMailAsync("test1@test.com", ct);

        // Assert
        Assert.Equal(new UserDto("test1@test.com", true, "Student"), result);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserNotFound_ReturnNull()
    {
        // Arrange
        mockUserRepository.GetIdentityUserAsync("test@test.com", ct).Returns((IdentityUser?)null);

        // Act
        var userDto = new UserDto("test@test.com", true, "Teacher");
        var result = await userService.UpdateUserAsync(userDto, ct);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserFound_ReturnUserDto()
    {
        // Arrange
        var userToUpdate = new IdentityUser();
        mockUserRepository.GetIdentityUserAsync("test@test.com", ct).Returns(userToUpdate);

        // Act
        var userDto = new UserDto("test@test.com", true, "Teacher");
        var result = await userService.UpdateUserAsync(userDto, ct);

        // Assert
        Assert.Equal(new UserDto("test@test.com", true, "Teacher"), result);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserFound_ReturnTrue()
    {
        // Arrange
        mockUserRepository.DeleteUserAsync("test@test.com", ct).Returns(true);

        // Act
        var result = await userService.DeleteUserAsync("test@test.com", ct);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserNotFound_ReturnFalse()
    {
        // Arrange
        mockUserRepository.DeleteUserAsync("test@test.com", ct).Returns(false);

        // Act
        var result = await userService.DeleteUserAsync("test@test.com", ct);

        // Assert
        Assert.False(result);
    }
}
