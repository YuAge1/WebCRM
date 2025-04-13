using Microsoft.AspNetCore.Mvc;
using WebCRM.Application.Interfaces;
using WebCRM.Domain.Entities;
using TaskStatus = WebCRM.Domain.Entities.TaskStatus;

namespace WebCRM.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ICrmService _crmService;

        public TasksController(ICrmService crmService)
        {
            _crmService = crmService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetAllTasks()
        {
            var tasks = await _crmService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskEntity>> GetTaskById(long id)
        {
            var task = await _crmService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasksByUserId(long userId)
        {
            var tasks = await _crmService.GetTasksByUserIdAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasksByStatus(TaskStatus status)
        {
            var tasks = await _crmService.GetTasksByStatusAsync(status);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskEntity>> CreateTask(TaskEntity task)
        {
            var createdTask = await _crmService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(long id, TaskEntity task)
        {
            try
            {
                await _crmService.UpdateTaskAsync(task);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(long id)
        {
            var result = await _crmService.DeleteTaskAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
} 