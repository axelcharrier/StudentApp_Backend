namespace StudentApp.Application.Models.Dto;

public sealed record UserDto(string Mail, bool IsMailConfirmed, string Role) { }