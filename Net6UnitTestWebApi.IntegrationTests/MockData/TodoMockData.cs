using System.Collections.Generic;
using Net6UnitTestWebApi.API.Models;

namespace Net6UnitTestWebApi.IntegrationTests.MockData;

public static class TodoMockData
{
    public static List<Todo> GetTodos()
    {
        return new List<Todo>
        {
            new(1, "Need To Go Shopping", true),
            new(2, "Cook Food", true),
            new(3, "Play Games", false),
        };
    }

    public static (Todo todo, int idTodo) AddTodo()
    {
        const int idNewTodo = 4;
        return (new Todo(idNewTodo, "Eat ice", true), idNewTodo);
    }
}