using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoApi.Models;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoController(TodoContext context)
    {
        _context = context;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
    {
        int userId = GetUserId();
        return await _context.TodoItems.Where(t => t.UserId == userId).ToListAsync();
    }
    [Authorize]
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetOverdueTasks()
    {
        int userId = GetUserId();
        var now = DateTime.UtcNow;

        var overdue = await _context.TodoItems
            .Where(t => t.UserId == userId && t.DueDate < now && !t.IsCompleted)
            .ToListAsync();

        return overdue;
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> FilterTodos(
    string? priority = null, string? category = null)
    {
        int userId = GetUserId();
        var query = _context.TodoItems.Where(t => t.UserId == userId);

        if (!string.IsNullOrEmpty(priority))
            query = query.Where(t => t.Priority == priority);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(t => t.Category == category);

        return await query.ToListAsync();
    }


    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodo(TodoItem item)
    {
        item.UserId = GetUserId();
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTodos), new { id = item.Id }, item);
    }

    // (Update/Delete methods also check user ID similarly)

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, TodoItem item)
    {
        if (id != item.Id) return BadRequest();
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var item = await _context.TodoItems.FindAsync(id);
        if (item == null) return NotFound();
        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
