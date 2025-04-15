using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCRM.Application.Abstractions;
using WebCRM.Application.Services;
using WebCRM.Domain.Models;

namespace WebCRM.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public TestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send-test-email")]
    public async Task<IActionResult> SendTestEmail([FromBody] TestEmailRequest request)
    {
        try
        {
            await _emailService.SendPromotionalEmail(
                request.Email,
                request.Subject,
                request.Body
            );
            return Ok(new { message = "Email sent successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("send-discount-notification")]
    public async Task<IActionResult> SendDiscountNotification([FromBody] TestDiscountRequest request)
    {
        try
        {
            await _emailService.SendDiscountNotification(request.Email, request.DiscountPercentage);
            return Ok(new { message = "Discount notification sent successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("send-login-notification")]
    public async Task<IActionResult> SendLoginNotification([FromBody] TestLoginRequest request)
    {
        try
        {
            await _emailService.SendLoginNotification(request.Email);
            return Ok(new { message = "Login notification sent successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public class TestEmailRequest
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class TestDiscountRequest
{
    public string Email { get; set; }
    public decimal DiscountPercentage { get; set; }
}

public class TestLoginRequest
{
    public string Email { get; set; }
} 