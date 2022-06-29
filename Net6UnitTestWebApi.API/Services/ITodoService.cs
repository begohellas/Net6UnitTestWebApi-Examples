using Net6UnitTestWebApi.API.Models;

namespace Net6UnitTestWebApi.API.Services;

public interface ITodoService
{
    Task<List<Todo>> GetAllAsync();
    Task<Todo?> GetById(int id);

    Task SaveAsync(Todo newTodo);
}