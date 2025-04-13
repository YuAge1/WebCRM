using Microsoft.AspNetCore.Mvc;
using WebCRM.Application.Interfaces;

namespace WebCRM.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ICrmService _crmService;

        public AnalyticsController(ICrmService crmService)
        {
            _crmService = crmService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnalytics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be later than end date");
            }

            var analytics = await _crmService.GetAnalyticsAsync(startDate, endDate);
            return Ok(analytics);
        }
    }
} 