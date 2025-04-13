using Microsoft.AspNetCore.Mvc;
using WebCRM.Application.Abstractions;
using WebCRM.Application.Models.Authentication;

namespace WebCRM.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ApiBaseController
{
    private readonly IAuthService _authService;

    public AccountsController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UserLoginDto userLoginDto)
    {
        try
        {
            var result = await _authService.Login(userLoginDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegisterDto userRegisterDto)
    {
        try
        {
            var result = await _authService.Register(userRegisterDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}