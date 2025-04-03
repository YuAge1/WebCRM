using Microsoft.AspNetCore.Mvc;
using WebCRM.Application.Abstractions;
using WebCRM.Application.Models.Authentication;

namespace WebCRM.WebApi.Controllers;

[Microsoft.AspNetCore.Components.Route("accounts")]
public class AccountsController(IAuthService authService) : ApiBaseController
{
    [HttpPost("login")]
    public async Task<ActionResult> Login(UserLoginDto userLoginDto)
    {
        var result = await authService.Login(userLoginDto);
        
        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegisterDto userRegisterDto)
    {
        var result = await authService.Register(userRegisterDto);
        
        return Ok(result);
    }
}