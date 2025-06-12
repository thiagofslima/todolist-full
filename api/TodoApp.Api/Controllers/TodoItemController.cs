using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Data;
using TodoApp.Api.Models;

namespace TodoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodoItemController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(TodoItem todoItem)
    {
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { Id = todoItem.Id }, todoItem);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetById(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        
        if (todoItem == null)
            return NotFound();
        
        return Ok(todoItem);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
    {
        return await _context.TodoItems.ToListAsync();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
            return BadRequest();
        
        var oldTodoItem = await _context.TodoItems.FindAsync(id);
        if (oldTodoItem == null)
            return NotFound();
        
        oldTodoItem.Name = todoItem.Name;
        oldTodoItem.IsCompleted = todoItem.IsCompleted;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500, "Erro ao salvar mudanças no banco.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
            return NotFound();
        
        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}