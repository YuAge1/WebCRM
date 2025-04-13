using Microsoft.AspNetCore.Mvc;
using WebCRM.Application.Interfaces;
using WebCRM.Domain.Entities;

namespace WebCRM.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly ICrmService _crmService;

        public ActivitiesController(ICrmService crmService)
        {
            _crmService = crmService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityEntity>>> GetAllActivities()
        {
            var activities = await _crmService.GetAllActivitiesAsync();
            return Ok(activities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityEntity>> GetActivityById(long id)
        {
            var activity = await _crmService.GetActivityByIdAsync(id);
            if (activity == null) return NotFound();
            return Ok(activity);
        }

        [HttpGet("entity/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<ActivityEntity>>> GetActivitiesByEntity(long entityId, string entityType)
        {
            var activities = await _crmService.GetActivitiesByEntityIdAsync(entityId, entityType);
            return Ok(activities);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ActivityEntity>>> GetActivitiesByUser(long userId)
        {
            var activities = await _crmService.GetActivitiesByUserIdAsync(userId);
            return Ok(activities);
        }

        [HttpPost]
        public async Task<ActionResult<ActivityEntity>> CreateActivity(ActivityEntity activity)
        {
            var createdActivity = await _crmService.CreateActivityAsync(activity);
            return CreatedAtAction(nameof(GetActivityById), new { id = createdActivity.Id }, createdActivity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(long id, ActivityEntity activity)
        {
            try
            {
                await _crmService.UpdateActivityAsync(activity);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(long id)
        {
            var result = await _crmService.DeleteActivityAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 