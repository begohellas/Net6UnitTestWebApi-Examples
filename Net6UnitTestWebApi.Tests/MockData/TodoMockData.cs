using System.Collections.Generic;
using Net6UnitTestWebApi.API.Models;

namespace Net6UnitTestWebApi.Tests.MockData;

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

    public static List<Todo> GetEmptyTodos()
    {
        return new List<Todo>();
    }

    public static Todo AddTodo()
    {
        return new Todo(4, "Eat ice", true);
    }

    public static Todo GetTodo()
        => new Todo(1, "Bye former colleagues", false);
}