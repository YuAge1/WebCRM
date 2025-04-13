using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebCRM.Application.Abstractions;
using WebCRM.Application.Models.Authentication;
using WebCRM.Domain.Entities;
using WebCRM.Domain.Exceptions;
using WebCRM.Domain.Models;
using WebCRM.Domain.Options;

public class AuthService : IAuthService
{
    private readonly AuthOptions _authOptions;
    private readonly UserManager<UserEntity> _userManager;

    public AuthService(
        IOptions<AuthOptions> authOptions,
        UserManager<UserEntity> userManager)
    {
        _authOptions = authOptions.Value;
        _userManager = userManager;
    }

    public async Task<UserResponse> Register(UserRegisterDto userRegisterDto)
    {
        if (await _userManager.FindByEmailAsync(userRegisterDto.Email) != null)
        {
            throw new DuplicateEntityException($"Email {userRegisterDto.Email} already exists");
        }

        var user = new UserEntity
        {
            Email = userRegisterDto.Email,
            PhoneNumber = userRegisterDto.Phone,
            UserName = userRegisterDto.Username,
            FirstName = userRegisterDto.FirstName,
            LastName = userRegisterDto.LastName,
            IsActive = true
        };

        var createUserResult = await _userManager.CreateAsync(user, userRegisterDto.Password);

        if (!createUserResult.Succeeded)
        {
            var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
            throw new Exception($"Failed to create user: {errors}");
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(user, RoleConsts.User);

        if (!addToRoleResult.Succeeded)
        {
            var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
            throw new Exception($"Failed to add user to role: {errors}");
        }

        var response = new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Roles = new[] { RoleConsts.User },
            Username = user.UserName,
            Phone = user.PhoneNumber,
            FirstName = user.FirstName,
            LastName = user.LastName
        };

        return GenerateToken(response);
    }

    public async Task<UserResponse> Login(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

        if (user == null)
        {
            throw new EntityNotFoundException($"User with email {userLoginDto.Email} not found");
        }
        
        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

        if (!checkPasswordResult)
        {
            throw new AuthenticationException("Invalid password");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var response = new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Roles = userRoles.ToArray(),
            Username = user.UserName,
            Phone = user.PhoneNumber,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
        return GenerateToken(response);
    }

    public UserResponse GenerateToken(UserResponse userRegisterModel)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authOptions.TokenPrivateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);
        
        var claims = new Dictionary<string, object>
        {
            {ClaimTypes.Name, userRegisterModel.Email!},
            {ClaimTypes.NameIdentifier, userRegisterModel.Id.ToString()},
            {JwtRegisteredClaimNames.Aud, "test"},
            {JwtRegisteredClaimNames.Iss, "test"}
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(userRegisterModel),
            Expires = DateTime.UtcNow.AddMinutes(_authOptions.ExpireIntervalMinutes),
            SigningCredentials = credentials,
            Claims = claims,
            Audience = "test",
            Issuer = "test"
        };

        var token = handler.CreateToken(tokenDescriptor);
        userRegisterModel.Token = handler.WriteToken(token);

        return userRegisterModel;
    }

    private static ClaimsIdentity GenerateClaims(UserResponse userRegisterModel)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, userRegisterModel.Email!));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, userRegisterModel.Id.ToString()));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, "test"));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, "test"));

        foreach (var role in userRegisterModel.Roles!)
            claims.AddClaim(new Claim(ClaimTypes.Role, role));

        return claims;
    }
}