namespace StudentApp.Application.Mappers;

using Riok.Mapperly.Abstractions;
using StudentApp.Application.Models.Dto;
using StudentApp.Domain.Entities;

[Mapper]
public static partial class UserMapper
{
    [MapProperty(nameof(User.UserName), nameof(UserDto.Mail))]
    [MapProperty(nameof(User.Role), nameof(UserDto.Role))]
    [MapProperty(nameof(User.EmailConfirmed), nameof(UserDto.IsMailConfirmed))]

    [MapperIgnoreSource(nameof(User.Id))]
    [MapperIgnoreSource(nameof(User.NormalizedUserName))]
    [MapperIgnoreSource(nameof(User.NormalizedEmail))]
    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.SecurityStamp))]
    [MapperIgnoreSource(nameof(User.ConcurrencyStamp))]
    [MapperIgnoreSource(nameof(User.PhoneNumber))]
    [MapperIgnoreSource(nameof(User.PhoneNumberConfirmed))]
    [MapperIgnoreSource(nameof(User.LockoutEnd))]
    [MapperIgnoreSource(nameof(User.LockoutEnabled))]
    [MapperIgnoreSource(nameof(User.AccessFailedCount))]
    [MapperIgnoreSource(nameof(User.Email))]
    [MapperIgnoreSource(nameof(User.IsMailConfirmed))]
    [MapperIgnoreSource(nameof(User.Mail))]
    [MapperIgnoreSource(nameof(User.TwoFactorEnabled))]
    public static partial UserDto ToUserDto(this User user);
}
