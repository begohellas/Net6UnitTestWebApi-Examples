using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Net6UnitTestWebApi.API.Models;
using Net6UnitTestWebApi.API.Services;

namespace Net6UnitTestWebApi.API.Controllers;

[Route("[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet(Name = nameof(GetAllAsync))]
    [ProducesResponseType(typeof(List<Todo>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _todoService.GetAllAsync();

        if (result.Count == 0)
        {
            return NoContent();
        }

        return Ok(result);
    }
    
    [HttpGet("{id:int}", Name = nameof(GetAsync))]
    [ProducesResponseType(typeof(Todo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var result = await _todoService.GetById(id);

        if (result is null)
        {
            return NotFound($"not found todo by Id {id.ToString()}");
        }

        return Ok(result);
    }

    [HttpPost(Name = nameof(SaveAsync))]
    [ProducesResponseType(typeof(Todo), StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> SaveAsync(Todo newTodo)
    {
        await _todoService.SaveAsync(newTodo);

        return Created($"/{newTodo.Id}", newTodo);
    }
}