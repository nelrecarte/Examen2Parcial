using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers;

[ApiController]
[Route("api/Task")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly TaskService _taskService;

    public TaskController(TaskService taskService) => _taskService = taskService;
    

    // POST: api/Task
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TaskDto dto)
    {
        try
        {
            var userId = User.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            
        
            var task = await _taskService.Create(dto, userId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/Task
    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        try
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            
            var tasks = await _taskService.GetTasksByUser(userId);

            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/Task/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(string id)
    {
        try
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            
            var deleted = await _taskService.DeleteTaskAsync(id, userId);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "La tarea no existe o no pertenece al usuario."
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private string? GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? User.FindFirst("sub")?.Value
               ?? User.FindFirst("userId")?.Value;
    }
}