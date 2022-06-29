using Microsoft.EntityFrameworkCore;
using Net6UnitTestWebApi.API.Data;
using Net6UnitTestWebApi.API.Models;

namespace Net6UnitTestWebApi.API.Services;

public class TodoService : ITodoService
{
    private readonly MyWorldDbContext _dbContext;

    public TodoService(MyWorldDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Todo>> GetAllAsync()
    {
        return await _dbContext.Todos.ToListAsync();
    }

    public async Task<Todo?> GetById(int id)
    {
        var todoId = new object[] { id };
        return await _dbContext.Todos.FindAsync(todoId);
    }

    public async Task SaveAsync(Todo newTodo)
    {
        _dbContext.Todos.Add(newTodo);
        await _dbContext.SaveChangesAsync();
    }
}