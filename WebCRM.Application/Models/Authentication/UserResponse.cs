﻿namespace WebCRM.Application.Models.Authentication;

public class UserResponse
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string[]? Roles { get; set; }
    public string? Token { get; set; }
}