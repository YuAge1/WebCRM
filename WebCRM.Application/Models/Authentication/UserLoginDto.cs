﻿namespace WebCRM.Application.Models.Authentication;

public record UserLoginDto(string Username, string Email, string Phone, string Password);