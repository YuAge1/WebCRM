﻿namespace WebCRM.Application.Models.Authentication;

public record UserRegisterDto(string Username, string Email, string Phone, string Password);